"use strict";

class Point {
    constructor(x, y) {
        this.x = x;
        this.y = y;
    }
}

class Rectangle {

    #x;
    #y;
    #width;
    #height;
    #top;
    #left;
    #right;
    #bottom;

    constructor(x, y, width, height) {
        this.#x = x;
        this.#y = y;
        this.#width = width;
        this.#height = height;
        this.#left = x;
        this.#top = y;
        this.#right = x + width;
        this.#bottom = y + height;
    }

    set width(width){
        this.#width = width;
        this.#right = this.#x + width;
    }

    get width(){
        return this.#width;
    }
    
    set height(height){
        this.#height = height;
        this.#bottom = this.#y + height;
    }

    get height(){
        return this.#height;
    }

    set x(x){
        this.#x = x;
        this.#left = x;
        this.#right = x + this.#width;
    }

    get x(){
        return this.#x;
    }

    set y(y){
        this.#y = y;
        this.#top = y;
        this.#bottom = y + this.#height;
    }

    get y(){
        return this.#y;
    }

    get left(){
        return this.#left;
    }

    get right(){
        return this.#right;
    }

    get top(){
        return this.#top;
    }

    get bottom(){
        return this.#bottom;
    }

    draw(ctx, strokeStyle, lineWidth, lineDash) {

        ctx.beginPath();
        ctx.rect(this.#x, this.#y, this.#width, this.#height);
        ctx.strokeStyle = strokeStyle;
        ctx.lineWidth = lineWidth;
        ctx.setLineDash(lineDash);
        ctx.stroke();

    }

    contains(x, y) {

        let lowerX = Math.min(this.#left, this.#right);
        let UpperX = Math.max(this.#left, this.#right);

        let lowerY = Math.min(this.#top, this.#bottom);
        let UpperY = Math.max(this.#top, this.#bottom);

        if (x > lowerX && x < UpperX && y > lowerY && y < UpperY) {
            return true
        }
    }

}

class ResizableRectangle {

    rectangle;
    ctx;
    outlineColor;
    lineWidth;
    lineDash;
    myCanvas;

    onMouseDownX = 0;
    onMouseDownY = 0;
    onMouseDownRectX = 0;
    onMouseDownRectY = 0;
    onMouseDownRectWidth = 0;
    onMouseDownRectHeight;
    mouseHoverX = 0;
    mouseHoverY = 0;
    scaleFactor = 0.1;

    dragging = false;
    resizing = false;

    static initialState = {

        rectangle: undefined,
        ctx: undefined,
        outlineColor: undefined,
        lineWidth: undefined,
        lineDash: undefined,
       
    };

    constructor(initialState) {

        this.rectangle = initialState.rectangle;
        this.ctx = initialState.ctx;
        this.outlineColor = initialState.outlineColor;
        this.lineWidth = initialState.lineWidth;
        this.lineDash = initialState.lineDash;
        this.enableInertialDrag = initialState.enableInertialDrag;

        this.myCanvas = this.ctx.canvas;

        this.myCanvas.addEventListener("mousedown", (e) => {
            this.handleMouseDown(e)
        });
        this.myCanvas.addEventListener("mousemove", (e) => {
            this.handleMouseMove(e)
        });
        this.myCanvas.addEventListener("mouseup", (e) => {
            this.handleMouseUp(e)
        });
        this.myCanvas.addEventListener("wheel", (e) => {
            this.handleZoom(e)
        });

    }

    drawRectangle() {
        this.rectangle.draw(this.ctx, this.outlineColor, this.lineWidth, this.lineDash);
    }

    drawHistogram(s, histo, drawAtY) {
        let maxY = s.maxValue, minY = s.minValue;
        let maxValue = histo.effectiveMaxValue, minValue = histo.effectiveMinValue;

        let maxPointY = ResizableRectangle.linearTransformY(this.rectangle.top + 1, maxValue, minY, maxY, this.rectangle.height - 2);
        let minPointY = ResizableRectangle.linearTransformY(this.rectangle.top + 1, minValue, minY, maxY, this.rectangle.height - 2);

        let histoBaseLength = maxPointY - minPointY;
        let barBaseSize = histoBaseLength / histo.numberOfClasses;

        let histoValues = histo.bucketValues;
        let maxHistoValue = Math.max(...histoValues);

        for (let i = 0; i < histo.numberOfClasses; i++) {
            let maxPoint = new Point(drawAtY, minPointY + i * barBaseSize);

            let rectangleWidth = (this.rectangle.width / 6) * (histoValues[i] / maxHistoValue);
            let rectangleHeight = barBaseSize;

            this.ctx.fillStyle = this.outlineColor;
            this.ctx.fillRect(maxPoint.x, maxPoint.y, rectangleWidth, rectangleHeight);

            this.ctx.strokeStyle = ResizableRectangle.getAnalogousColors(this.outlineColor);
            this.ctx.strokeRect(maxPoint.x, maxPoint.y, rectangleWidth, rectangleHeight);
        }
    }

    drawChart(s) {

        const pointsList = s.coordinates;
        const trajectoriesColor = s.trajectoriesColor;

        this.drawRectangle();

        for (let k = 0; k < s.M; k++) {
            const scaledPoints = new Array(s.N);

            for (let i = 0; i < s.N; i++) {
                scaledPoints[i] = new Point(
                    ResizableRectangle.linearTransformX(this.rectangle.left + 1, pointsList[k][i].x, 0, s.N - 1, this.rectangle.width - 2),
                    ResizableRectangle.linearTransformY(this.rectangle.top + 1, pointsList[k][i].y, s.minValue, s.maxValue, this.rectangle.height - 2)
                );
            }

            this.ctx.strokeStyle = trajectoriesColor[k];
            this.ctx.beginPath();
            this.ctx.moveTo(scaledPoints[0].x, scaledPoints[0].y);
            for (let i = 1; i < s.N; i++) {
                this.ctx.lineTo(scaledPoints[i].x, scaledPoints[i].y);
            }
            this.ctx.stroke();
        }

        this.drawHistogram(s, s.endValuesHistogram, this.rectangle.right + 2);
        this.drawHistogram(s, s.midValuesHistogram, ResizableRectangle.linearTransformX(this.rectangle.left + 1, s.midValuesHistogram.calculationPoint, 0, s.N - 1, this.rectangle.width - 2));
    }

    static rgbToHsv(r, g, b) {
        r /= 255, g /= 255, b /= 255;
        let max = Math.max(r, g, b), min = Math.min(r, g, b);
        let h, s, v = max;
        let d = max - min;
        s = max == 0 ? 0 : d / max;
        if (max == min) {
            h = 0; // achromatic
        } else {
            switch (max) {
                case r: h = (g - b) / d + (g < b ? 6 : 0); break;
                case g: h = (b - r) / d + 2; break;
                case b: h = (r - g) / d + 4; break;
            }
            h /= 6;
        }
        return [h * 360, s * 100, v * 100];
    }

    static hsvToRgb(h, s, v) {
        let r, g, b;
        let i = Math.floor(h / 60);
        let f = h / 60 - i;
        let p = v * (1 - s);
        let q = v * (1 - f * s);
        let t = v * (1 - (1 - f) * s);
        switch (i % 6) {
            case 0: r = v, g = t, b = p; break;
            case 1: r = q, g = v, b = p; break;
            case 2: r = p, g = v, b = t; break;
            case 3: r = p, g = q, b = v; break;
            case 4: r = t, g = p, b = v; break;
            case 5: r = v, g = p, b = q; break;
        }
        return [Math.round(r * 255), Math.round(g * 255), Math.round(b * 255)];
    }

    static getAnalogousColors(primaryColor) {
        // Convert the primary color to HSV
        let [h, s, v] = ResizableRectangle.rgbToHsv(primaryColor.r, primaryColor.g, primaryColor.b);

        // Compute the hues of the analogous colors
        let analogousHue1 = (h + 30) % 360;

        // Convert the analogous HSV colors back to RGB
        let analogousColor1Rgb = ResizableRectangle.hsvToRgb(analogousHue1, s, v);

        return { r: analogousColor1Rgb[0], g: analogousColor1Rgb[1], b: analogousColor1Rgb[2] };
    }



    handleMouseDown(e) {

        const transformedY = ctx.canvas.height - e.clientY;

        if (this.rectangle.contains(e.clientX, transformedY)) {
            this.onMouseDownX = e.clientX;
            this.onMouseDownY = transformedY;

            this.onMouseDownRectX = this.rectangle.x;
            this.onMouseDownRectY = this.rectangle.y;

            this.onMouseDownRectWidth = this.rectangle.width;
            this.onMouseDownRectHeight = this.rectangle.height;

            if (e.which == 1) {
                this.dragging = true;
            }
            else if (e.which == 3) {
                this.resizing = true;
            }
        }
    }

    handleMouseMove(e) {

        this.mouseHoverX = e.clientX;
        this.mouseHoverY = ctx.canvas.height - e.clientY;

        const deltaX = e.clientX - this.onMouseDownX;
        const deltaY = ctx.canvas.height - e.clientY - this.onMouseDownY;

        if (this.dragging) {
            this.rectangle.x = this.onMouseDownRectX + deltaX;
            this.rectangle.y = this.onMouseDownRectY + deltaY;
        }
        else if (this.resizing) {
            this.rectangle.width = this.onMouseDownRectWidth + deltaX;
            this.rectangle.height = this.onMouseDownRectHeight + deltaY;
        }
    }

    handleMouseUp(e) {
        this.dragging = false;
        this.resizing = false;
    }

    handleZoom(e) {

        if (this.rectangle.contains(this.mouseHoverX, this.mouseHoverY)) {
            this.onMouseDownRectX = this.rectangle.x;
            this.onMouseDownRectY = this.rectangle.y;

            this.rectangle.width += -e.deltaY * this.scaleFactor;
            this.rectangle.height += -e.deltaY * this.scaleFactor;

            this.rectangle.x = this.onMouseDownRectX - ((-e.deltaY * this.scaleFactor) / 2);
            this.rectangle.y = this.onMouseDownRectY - ((-e.deltaY * this.scaleFactor) / 2);
        }

    }



    static linearTransformX(offset, X, minX, maxX, width) {
        return offset + (width * ((X - minX) / (maxX - minX)));
    }

    static linearTransformY(offset, Y, minY, maxY, height) {
        return ((Y - minY) / (maxY - minY)) * height + offset;
    }

}