# Snakeio
Изработено од Бобан Богоевски 161103.

## За играта
C# верзија на Slitherio. Користејќи го глушецот за движење на змијата, собирај ја храната и одбегнувај ги другите змии. 

### Генерирање на храната и играчот

Како што знаеме, на мапата се наоѓаат многу кругови со храна кои играчот или другите змии можат да ги собираат за поени и издолжување. Бидејќи големината на светот на играта (2\*2^32)x(2\*2^32), потребно би било да се генерира голема количина на храна, успорувајќи ја играта. Затоа решив да генерирам храна во областа околу змијата колку што е голем прозорецот. Кога змијата се придвижува во одредена насока, потребно е и генерирање на нова храна. Затоа при движење на змијата во некоја насока, храната која повеќе не се гледа се брише и нова храна се генерира во насоката на движење на змијата.

Телото на змијата претставува листа на Point каде секој дел од телото има своја xy координата. Таа е генерирана во центарот на мапата, со нејзината глава нацртана во центарот на прозорецот, додека пак другите делови од телото се нацртани преку растојанието помеѓу делот од телото и главата на змијата.

### Движење на змиите

Начинот на кој змијата се придвижува е преку следење на курсорот. За да го добиеме аголот на придвижување на змијата, при секој настан MouseMove, го пресметуваме аголот помеѓу главата на змијата односно центарот на екранот и позицијата на глушецот на прозорецот со следниов код:

<code>	

	public void changeAngle(Point mousePos, int worldWidth, int worldHeight)
		
		{
	
			int centerX = (int)(worldWidth / 2);
	
			int centerY = (int)(worldHeight / 2);
	
			angle = -Math.Atan2((1d * centerY - mousePos.Y), (1d * centerX - mousePos.X));
		}
</code>		


Притоа, за добивање на следната позиција на главата, ги користиме следниве формули:

<code>

	forceX = -(int)(force * Math.Cos(angle));
	forceY = (int)(force * Math.Sin(angle));
	posX = body[0].X + forceX;
	posY = body[0].Y + forceY;
	body[0] = new Point(posX,posY);
</code>

каде force ја одредува брзината на движење на змијата и е еднаква на 10. Овој код за придвижување го позајмив од https://github.com/bibhuticoder/snake.io.

### AI Snakes

На почетокот се генерираат 4 AI змии во близина на играчот. Начинот на кои се движат за разлика од нашата змија е така што одбираат најповолна храна по најдобар score ( алгоритам се покажува несовршен ) и се движат во таа насока.


### Special food

Постојат 2 специјални типа на храна(power ups):


<img src="https://i.imgur.com/P10RfmS.png" style="width:50px;"> 
Кафе - за 8 секунди, змијата се движи побрзо.

<img src="https://i.imgur.com/B3piYBp.png" style="width:50px;"> 
Штит - за 10 секунди, змијата е непобедлива и секоја змија која ќе ја допре умира. Ако настане колизија помеѓу две змии, тогаш се третира исто како и нормална колизија.

Овие специјални типови на храна се чуваат во посебна класа SpecialFood и го содржат:
* времето на траење на ефектот - duration
* позицијата на храната
* змијата која ја изела
* дали е изедена или не

Откако ќе бидат изедени овие power ups, effectTimer го намалува времето на ефектот со методот effectTimerTick на секои 10ms. Ако змијата има веќе постоечки ефект, тогаш не може да изеде Special Food се додека тајмерот не заврши.



