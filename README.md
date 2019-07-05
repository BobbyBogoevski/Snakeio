# Snakeio
Изработено од Бобан Богоевски 161103.

<h1>За играта</h1>
C# верзија на Slitherio. Користејќи го глушецот за движење на змијата, собирај ја храната и одбегнувај ги другите змии. 

<h1>Генерирање на храната и играчот</h1>

Како што знаеме, на мапата се наоѓаат многу кругови со храна кои играчот или другите змии можат да ги собираат за поени и издолжување. Бидејќи големината на светот на играта (2*2^32)x(2*2^32), потребно би било да се генерира голема количина на храна, успорувајќи ја играта. Затоа решив да генерирам храна во областа околу змијата колку што е голем прозорецот. Кога змијата се придвижува во одредена насока, потребно е и генерирање на нова храна. Затоа при движење на змијата во некоја насока, храната која повеќе не се гледа се брише и нова храна се генерира во насоката на движење на змијата.

<code>
	            void generateNewFood(int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		 {
			Point f;
			//south-east
			double angle = (player.angle * 180 / Math.PI) + 180;
			if (angle <= 340 && angle >= 290) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY)); //east
					
				} else {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 5, viewEndY + 35));//south
					
				}
			}
			//north-east
			else if (angle >= 20 && angle <= 70) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 35, viewStartY - 5));//north
					
				} else {
					f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY));//east
					
				}
				
			}
			//south-west
			else if (angle >= 200 && angle <= 250) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 5, viewEndY + 35));//south
					
				} else {
					f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));//west
					
				}
				
			}
			//north-west
			else if (angle >= 110 && angle <= 160) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 35, viewStartY - 5));//north
					
				} else {
					f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));//west
					
				}
			}
			//east
			else if (angle < 20 || angle > 340) {
				f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY));
				
			}
			//west
			else if (angle > 160 && angle < 200) {
				f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));
				
			}
			//south
			else if (angle > 250 && angle < 290) {
				f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 5, viewEndY + 35));
				
			}
			//north
			else {
				f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 35, viewStartY - 5));
				
			}
			
			
			food.Add(f);
		}
</code>

<h2>Power ups</h2>
<img src="https://i.imgur.com/P10RfmS.png" style="width:50px;"> Кафе - за 8 секунди, змијата се движи побрзо.
<img src="https://i.imgur.com/B3piYBp.png" style="width:50px;"> Штит - за 10 секунди, змијата е непобедлива и секоја змија која ќе ја допре умира. Ако настане колизија помеѓу две змии, тогаш се третира исто како и нормална колизија.



