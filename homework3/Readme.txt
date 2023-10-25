Exercise 1

Part a
M systems are subject to a series of N attacks. On the x-axis, we indicate the attacks and on the Y-axis we
simulate the accumulation of a "security score" (-1, 1), where the score is -1 if the system is penetrated
and 1 if the system was successfully "shielded" or protected. Simulate the score "trajectories" for all systems,
assuming, for simplicity, a constant penetration probability p at each attack.

Part b
Same as before, but simulate the cumulated frequency, say f, of penetration. Do the same with the relative
frequency f/number of attacks and the "normalized" ratio: f/ √number of attacks.


For any of the above 4 charts (which will be actually an instance of a unique "object", from a coder's point of view), plot
a vertical histogram at some point x (day or attack number, user parameter) and at the last abscissa
value and make your personal considerations on the shape of the distributions.
Make sure that each animation is enclosed into a "frame" (a rectangle) resizable by the user, by using the mouse
(you can make a separate, reusable, "ResizableRectangle" object for that).

Discussion point:

Is what you see what you expected? What about the averages of the distributions and the shapes of the histograms:
do you see regularities, differences and can you attempt to explain what you see or guessing what are
the "theoretical" limit distribution, when as N increases, and you can make the distribution simulation "more detailed" by increasing M ?


--------------------------------------
Optional Part c

Given M computer systems. For each system, consider N days, where an attack can happen with probability p.
[You can allow p to change over systems or over days, if you wish, and note if there are differences in the (asymptotic) distribution.]
Chart the cumulative number of attacks at each day (all systems) or for each system (all days) with an animation
that shows, at any time, either the total number of attacks for each system or the attacks in each day for all systems.

Optional Part d

Do the same as in the previous part but, instead of counting the days with attacks, count the actual attacks each day which
we assume to be (0, 1, ..., k) with respective (constant wrt time) probabilities (p0, ..., pk).

--------------------------------------

Exercise 2

Recall briefly the definition and math notions relevant to "probability space" and make some simple examples, indicating among the triple of the space the meaning of each element in your particular example.
If you wanted to model probabilistically the homework Exercise 1, explain what are the 3 sets of your probability space and their elements, in this case.

-----------------------