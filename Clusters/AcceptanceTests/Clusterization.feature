Feature: Clusterization
	Кластеризация точек, введённых на графике

Scenario Outline: Succesful Clusterization
	Given I started application
	And I entered points <points>
	And I entered minimal number of points <numPoints>
	And I entered epsilon <epsilon>
	And I chose metric <metric>
	When I pressed clusterize
	And I pressed results
	Then There must be clusters <clusters> and noise <noise>
Examples: 
| points                                                                            | numPoints | epsilon | metric | clusters                                                           | noise                |
| {(0;0),(1;0),(0.5;0.1),(-1;-0.2),(10;10),(10.2;9.9),(11.1;10),(100;100),(-10;10)} | 2         | 2       | LP(2)  | [{(0;0),(1;0),(0.5;0.1),(-1;-0.2)},{(10;10),(10.2;9.9),(11.1;10)}] | {(100;100),(-10;10)} |
| {(1;1),(5;5),(7;7)}                                                               | 2         | 1       | Sup    | []                                                                 | {(1;1),(5;5),(7;7)}  |