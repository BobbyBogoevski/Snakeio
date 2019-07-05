# Snakeio
Изработено од Бобан Богоевски 161103.

<h1>За играта</h1>
C# верзија на Slitherio. Користејќи го глушецот за движење на змијата, собирај ја храната и одбегнувај ги другите змии. 

<h1>Генерирање на храната и играчот</h1>

Како што знаеме, на мапата се наоѓаат многу кругови со храна кои играчот или другите змии можат да ги собираат за поени и издолжување. Бидејќи големината на светот на играта (2*2^32)x(2*2^32), потребно би било да се генерира голема количина на храна, успорувајќи ја играта. Затоа решив да генерирам храна во областа околу змијата колку што е голем прозорецот. Кога змијата се придвижува во одредена насока, потребно е и генерирање на нова храна. Затоа при движење на змијата во некоја насока, храната која повеќе не се гледа се брише и нова храна се генерира во насоката на движење на змијата.

Змијата е генерирана во центарот на мапата, со нејзината глава нацртана во центарот на прозорецот.

<code>
public void Draw(Graphics g, int worldWidth, int worldHeight)
{
	
	SolidBrush br = new SolidBrush(color);
	
	int centerX = (int)(worldWidth / 2);
	int centerY = (int)(worldHeight / 2);
	g.FillEllipse(br, centerX - SNAKEHEAD_RADIUS, centerY - SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS);
	double angleDeg = (angle * 180 / Math.PI) + 180;
	g.FillPie(new SolidBrush(Color.White), centerX - SNAKEHEAD_RADIUS, centerY - SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, -(float)(angleDeg + 50), 30f);
	g.FillPie(new SolidBrush(Color.White), centerX - SNAKEHEAD_RADIUS, centerY - SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, -(float)(angleDeg - 30), 30f);
	
	for (int i = 1; i < body.Count; i++) {
		int distanceX = body[i].X - body[0].X;
		int distanceY = body[i].Y - body[0].Y;
		if (distanceX > (20 * i))
			distanceX = -(int.MaxValue - body[i].X + body[0].X);
		if (distanceY > (20 * i))
			distanceY = -(int.MaxValue - body[i].Y + body[0].Y);
		if (distanceX < (-20 * i))
			distanceX = (int.MaxValue - body[0].X + body[i].X);
		if (distanceY < (-20 * i))
			distanceY = (int.MaxValue - body[0].Y + body[i].Y);
		
		Point bodypos = new Point(centerX + distanceX, centerY + distanceY);
		g.FillEllipse(br, bodypos.X - SNAKEHEAD_RADIUS, bodypos.Y - SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS);
	}
	
	
	br.Dispose();
	
}
</code>


<h2>Power ups</h2>
<img src="https://i.imgur.com/P10RfmS.png" style="width:50px;"> Кафе - за 8 секунди, змијата се движи побрзо.
<img src="https://i.imgur.com/B3piYBp.png" style="width:50px;"> Штит - за 10 секунди, змијата е непобедлива и секоја змија која ќе ја допре умира. Ако настане колизија помеѓу две змии, тогаш се третира исто како и нормална колизија.



