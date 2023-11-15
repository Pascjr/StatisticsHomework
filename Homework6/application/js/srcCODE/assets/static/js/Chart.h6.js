"use strict";

class Chart {

    constructor(){
        return this;
    }

    Instance(N, numberOfTrajectories, scaleFactor, yFunc, k, safeGuard) {
            this.n = N;
            this.numberOfTrajectories = numberOfTrajectories;
            this.scaleFactor = scaleFactor;
            this.coordinates = [];
            this.aggregatedCoordinates = [];
            this.values = Array(this.numberOfTrajectories).fill().map(() => Array(this.n));
            this.aggregatedValues = Array(this.n);
            this.YFunc = yFunc;
            this.maxValuesBeforeSafe = Array(this.numberOfTrajectories);
            this.k = k;
            this.trajectoriesColor = Array(this.numberOfTrajectories + k);

            for (let j = numberOfTrajectories; j < numberOfTrajectories + k - 1; j++) {
                this.trajectoriesColor[j] = "Black";
            }

            this.aggregatedTrajectoryColor = `rgb(${Math.floor(Math.random() * 256)}, ${Math.floor(Math.random() * 256)}, ${Math.floor(Math.random() * 256)})`;
            this.maxValue = k * 10 + 10;
            this.minValue = safeGuard - 10;
            this.maxAggregatedValue = 0;
            this.minAggregatedValue = 0;

            this.Simulate();

            this.CalculateProbabilities(k, safeGuard);

            this.calculateThresholdsLines(k, safeGuard);
            this.trajectoriesColor[this.numberOfTrajectories + k - 1] = "Blue";

            return this;
        }

    ShallowInstance = function(self, N, numberOfTrajectories, scaleFactor, yFunc, coordinates, aggCoordinates, values, aggValues, maxValues, min, max, aggMin, aggMax, k, safeGuard) {
            self.n = N;
            self.numberOfTrajectories = numberOfTrajectories;
            self.scaleFactor = scaleFactor;
            self.coordinates = coordinates.map(coord => coord.map(point => new Point(point.x, point.y)));
            self.maxValuesBeforeSafe = maxValues;
            self.k = k;
            self.aggregatedCoordinates = aggCoordinates.map(point => new Point(point.x, point.y));
            self.values = values;
            self.aggregatedValues = aggValues;
            self.YFunc = yFunc;
            self.trajectoriesColor = Array(self.numberOfTrajectories + k).fill("White");

            for (let j = 0; j < self.trajectoriesColor.length; j++) {
                if (j >= self.numberOfTrajectories) {
                    self.trajectoriesColor[j] = "Black";
                }
            }

            self.aggregatedTrajectoryColor = `rgb(${Math.floor(Math.random() * 256)}, ${Math.floor(Math.random() * 256)}, ${Math.floor(Math.random() * 256)})`;
            self.maxValue = k * 10 + 10 > max ? k * 10 + 10 : max;
            self.minValue = safeGuard - 10 < min ? safeGuard - 10 : min;
            self.maxAggregatedValue = aggMax;
            self.minAggregatedValue = aggMin;

            self.CalculateProbabilities(k, safeGuard);

            self.calculateThresholdsLines(k, safeGuard);
            self.trajectoriesColor[self.numberOfTrajectories + k - 1] = "Blue";

            return self;
        }

    calculateThresholdsLines(k, safeGuard) {
        let line;

        for (let i = 2; i <= k; i++) {
            line = [new Point(0, i * 10), new Point(this.n - 1, i * 10)];
            this.coordinates.push(line);
        }

        line = [new Point(0, safeGuard), new Point(this.n - 1, safeGuard)];
        this.coordinates.push(line);
    }

    Simulate() {
        for (let k = 0; k < this.numberOfTrajectories; k++) {
            let points = Array(this.n).fill().map(() => new Point(0, 0));

            for (let i = 0; i < this.n; i++) {
                this.values[k][i] = this.YFunc(this.values, k, i);
                points[i].x = i / this.scaleFactor;
                points[i].y = this.values[k][i];

                if (this.values[k][i] > this.maxValue) {
                    this.maxValue = this.values[k][i];
                } else if (this.values[k][i] < this.minValue) {
                    this.minValue = this.values[k][i];
                }

                if (i === 0) {
                    this.maxValuesBeforeSafe[k] = this.values[k][i];
                } else {
                    if (this.values[k][i] > this.maxValuesBeforeSafe[k]) {
                        this.maxValuesBeforeSafe[k] = this.values[k][i];
                    }
                }
            }
            this.coordinates.push(points);
            this.trajectoriesColor[k] = "White";
        }

        for (let i = 0; i < this.n; i++) {
            this.aggregatedValues[i] = 0;
            let point = new Point(i / this.scaleFactor, 0);

            for (let k = 0; k < this.numberOfTrajectories; k++) {
                let tmp = (this.aggregatedValues[i] += this.values[k][i]);
                if (tmp > this.maxAggregatedValue) {
                    this.maxAggregatedValue = tmp;
                } else if (tmp < this.minAggregatedValue) {
                    this.minAggregatedValue = tmp;
                }
            }

            point.y = this.aggregatedValues[i];
            this.aggregatedCoordinates.push(point);
        }
    }

    CalculateProbabilities(k, safeThreshold) {
        let pThresholds = Array(k - 1).fill().map((_, i) => (i + 2) * 10);
        let pThresholdsFreqs = Array(k - 1).fill(0);
        this.probabilities = Array(k - 1).fill(0);

        for (let system = 0; system < this.numberOfTrajectories; system++) {
            let didIncrement = Array(k - 1).fill(false);
            let penetrated = false;

            for (let value = 0; value < this.n; value++) {
                if (this.values[system][value] <= safeThreshold) {
                    if (!penetrated)
                        this.trajectoriesColor[system] = "Green";

                    break;
                } else if (this.values[system][value] >= pThresholds[0]) {
                    for (let j = 0; j < pThresholds.length; j++) {
                        if (!didIncrement[j] && this.values[system][value] >= pThresholds[j]) {
                            pThresholdsFreqs[j]++;
                            didIncrement[j] = true;
                        }
                    }

                    this.trajectoriesColor[system] = "Red";
                    penetrated = true;
                }
            }
        }

        for (let i = 0; i < this.probabilities.length; i++) {
            this.probabilities[i] = pThresholdsFreqs[i] / this.numberOfTrajectories;
        }
    }

    getShallowCopy(k, safeGuard) {

        const c = new Chart();


        //console.log(this.coordinates);

        return this.ShallowInstance(c, this.n, this.numberOfTrajectories, this.scaleFactor, this.YFunc, this.coordinates, this.aggregatedCoordinates, this.values, this.aggregatedValues, this.maxValuesBeforeSafe,
            this.minValue, this.maxValue, this.minAggregatedValue, this.maxAggregatedValue, k, safeGuard);
    }

}

