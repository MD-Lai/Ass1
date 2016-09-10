# Ass1
Graphics and Interaction diamond square algorithm terrain generator.
Written By Marvin Lai.
Student Number 754672.

Should work by just opening mainScene and pressing play
Controls: w/s for forward/backward, a/d for left/rigt, q/e for rotate anticlockwise/clockwise, lshift/lctrl for up/down, mouse controls for look

Works on standard diamond square algorithm,
heightmap is a 2d array of floats always of a max size (2^n) + 1 where n <= 7 this may be referred to as the grid from now on,
sets 4 corners to be a random number within the specified range modifiable in unity,
sets the centre of the square to be an average of the 4 corner values + some random noise,
each mid point of the edge is then an average of the 4 points up down left right of that point,
in the case where there is no point either above, below, left, or right of that point, only the average of the existing points is, taken
now we have sub-squares within the original square and hoorah we can recurse.

the DiamondSquare recursive implementation:
the square step:
begin with a step equal to (int)size/2 and step through each row and column denoted by step, incrementing by size,
starting with row and col being one step in,
calculate the average of the surrounding points and add some noise.

the diamond step:
begin with the same step size as above, starting at the first row, and only incrementing by step this time,
with the column, begin with (row+step) % size and increment by size, this will toggle between one step in and no steps in,
and for each point denoted by the row and columns, calculate the average of the surrounding points and add some noise.

once all the above is done, call the function again with size = size/2,
also absolutely crucially, the noise added has to decrease with each level of recursion, such that vertices generated later will be less random and conform to the environment surrounding it more.

using the heightmap generated, the points are then made into a list of vertices where each x and z value is separated by a specified spacing parameter,
with the y value being the value of the corresponding index on the heightmap
the list of vertices are then connected as triangles square by square, where each square set of vertices are grouped as 2 triangles.

colors are simply assigned as "if( within range specified by <terrainType> ) give the vertex the colour of terrainType plus a little randomness to look a bit nicer".

the mesh is then passed to the phong shader as completed in the workshop to render.

Water is created as a plane at 95% the upper limit of the "sand" boundary so a little bit of sand does show through,
the color is a 75% opaque pure blue and rendered by the standard unity transparent shader. 

Code was mostly written by yours truly, but hugely assisted by the following site: 
http://www.playfuljs.com/realistic-terrain-in-130-lines/
and an integer exponent function adapted to limit the exponent from:
http://stackoverflow.com/questions/383587

As a leftover from testing, bump the ball floating around the world origin for entertainment.
