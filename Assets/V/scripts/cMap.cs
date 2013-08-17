using UnityEngine;
using System.Collections;

public class cMap : MonoBehaviour
{
	const byte FLOOR_CORRIDOR			= 1;
	const byte FLOOR_CUBICLE			= 2;
	const byte FLOOR_LUNCH				= 3;
	const byte FLOOR_KITCHEN			= 4;
	const byte FLOOR_TOILET				= 5;
	const byte FLOOR_IT						= 6;
	const byte FLOOR_SERVERS			= 7;
	const byte FLOOR_CONFERENCE		= 8;
	const byte FLOOR_CLOSET				= 9;
	const byte FLOOR_COPY					= 10;
	const byte FLOOR_FREEZER			= 11;	// Attach to Kitchen (which is attached to Lunch Room) :D
	const byte FLOOR_MAIL					= 12;
	const byte FLOOR_SUPPLIES			= 13;
	const byte FLOOR_SECRET				= 14;

	const byte FLOOR_RELAX							= 15;
	const byte FLOOR_BAR								= 16;
	const byte FLOOR_CEO								= 17;
	const byte FLOOR_MIDDLE_MANAGEMENT 	= 18;

	const byte FLOOR_START					= 252;
	const byte FLOOR_END						= 253;
	const byte FLOOR_UNKNOWN				= 254;
		
	GameObject[] pfbFloor = new GameObject[256];
	GameObject[] pfbWallX = new GameObject[8];
	GameObject[] pfbWallY = new GameObject[8];

	int level, levelWidth, levelHeight;
	int[,] tunnelMap;
	byte[,] floorMap;
	byte[,] wallMapX;
	byte[,] wallMapY;
	int[,] pathMap;
	int tunnelX, tunnelY, tunnelD, oldTunnelD, tunnelIndex;
	int scx1, scy1, scd1, scx2, scy2, scd2;
	int lastPathWeight;
	int tunnelRoomIndexStart, levelSecretRoomCount = 0;
	
	
	void Start()
	{
		LoadFloorPrefabs();
		LoadWallPrefabs();
		
		GenerateMap(2);
		DrawMap();
	}
	
	void GenerateMap(int in_level)
	{
		// level parameters
		level = in_level;
		levelWidth = 16 + level / 5 + Random.Range(0,3)-1;
		levelHeight = 16 + level / 5 + Random.Range(0,3)-1;
		levelSecretRoomCount = 0;

		int maxCorridorTunnel1 = 6 + level/8;
		int maxCorridorTunnel2 = 19 + level/6;
		int minCorridorLength1 = 3 + level/50;
		int maxCorridorLength1 = 6 + level/30;
		int minCorridorLength2 = 3 + level/50;
		int maxCorridorLength2 = 6 + level/30;
		int maxCubiclePerRow = 6 + level/10;
		int maxCubicleRows = 5 + level/6;
		int maxConferencePerRow = 1 + level/15;
		int maxConferenceRows = 2 + level/7;
		int maxToilets = 1 + level/14 + Random.Range(0,2);
		int maxClosets = 2 + level/8 + Random.Range(0,4);
		int lunchRoomSizeX = 2 + level/25 + Random.Range(0,2);
		int lunchRoomSizeY = 2 + level/25 + Random.Range(0,2);
		int kitchenSizeX = 2 + level/55 + Random.Range(0,2);
		int kitchenSizeY = 1 + level/55 + Random.Range(0,2);
		
		// init
		floorMap = new byte[levelWidth,levelHeight];
		pathMap = new int[levelWidth,levelHeight];
		tunnelMap = new int[levelWidth,levelHeight];
		wallMapX = new byte[levelWidth+1,levelHeight+1];
		wallMapY = new byte[levelWidth+1,levelHeight+1];
		
		// clear
		for(int j=0; j<levelHeight+1; j++)
			for(int i=0; i<levelWidth+1; i++)
			{
				wallMapX[i,j] = 0;
				wallMapY[i,j] = 0;
			}
		for(int j=0; j<levelHeight; j++)
			for(int i=0; i<levelWidth; i++)
			{
				floorMap[i,j] = 0;
				pathMap[i,j] = 0;
				tunnelMap[i,j] = 0;
			}
		tunnelIndex = 0;
		
		// staircase 1 - start point
		SetStartPoint(Random.Range(2,levelWidth-2), Random.Range(2,levelHeight-2), Random.Range(0,4));
		tunnelIndex = 2;

		if (scd1 == 2)	{	tunnelX = scx1;	tunnelY = scy1+1;	}
		else if (scd1 == 3)	{	tunnelX = scx1+1;	tunnelY = scy1;	}
		else if (scd1 == 0)	{	tunnelX = scx1;	tunnelY = scy1-1;	}
		else if (scd1 == 1)	{	tunnelX = scx1-1;	tunnelY = scy1;	}
		for (int l=0; l < 150000; l++)
		{
			tunnelD = Random.Range(0,4);
			if ((tunnelD == 0) && (scd1 != 2))	break;
			if ((tunnelD == 1) && (scd1 != 3))	break;
			if ((tunnelD == 2) && (scd1 != 0))	break;
			if ((tunnelD == 3) && (scd1 != 1))	break;
		}
		
		for(int ken=0; ken < maxCorridorTunnel1; ken++)
		{
			TunnelCorridor(Random.Range(minCorridorLength1,maxCorridorLength1));
			if (ken < 3)	SwitchTunnelDirection();
		}

		for(int ken=0; ken < maxCorridorTunnel2; ken++)
		{
			for (int l=0; l<5000; l++)
			{
				tunnelX = Random.Range(0,levelWidth);
				tunnelY = Random.Range(0,levelHeight);
				tunnelD = Random.Range(0,4);
				if (tunnelMap[tunnelX,tunnelY] > 2)
				{
					lastPathWeight = pathMap[tunnelX, tunnelY];
					for(int ken2 = 0; ken2 < maxCorridorTunnel1; ken2++)
					{
						TunnelCorridor(Random.Range(minCorridorLength2,maxCorridorLength2));
						if (ken2 < 5)	SwitchTunnelDirection();
					}
					break;
				}
			}
		}

		int debugWeight = -1, debugX = -1, debugY = -1;
		for (int i=0; i<levelWidth; i++)
			for (int j=0; j<levelHeight; j++)
				if (pathMap[i,j] > debugWeight)
				{
					debugWeight = pathMap[i,j];
					debugX = i;
					debugY = j;
				}
	
		if (level < 100)
		{
			for(int l=0; l<150000; l++)
			{
				int dirren = Random.Range(0,4);
				if (dirren == 0)	if (debugY < levelHeight-1)	if (tunnelMap[debugX,debugY+1] == 0)	{	SetEndPoint(debugX,debugY+1,0);	break;	}
				if (dirren == 1)	if (debugX < levelWidth-1)	if (tunnelMap[debugX+1,debugY] == 0)	{	SetEndPoint(debugX+1,debugY,1);	break;	}
				if (dirren == 2)	if (debugY > 0)	if (tunnelMap[debugX,debugY-1] == 0)	{	SetEndPoint(debugX,debugY-1,2);	break;	}
				if (dirren == 3)	if (debugX > 0)	if (tunnelMap[debugX-1,debugY] == 0)	{	SetEndPoint(debugX-1,debugY,3);	break;	}
			}
		}

		tunnelIndex++;
		tunnelRoomIndexStart = tunnelIndex;
	
		PlaceRooms(FLOOR_MAIL,FLOOR_CORRIDOR, 2,2, 1,1);	// Mail room

		if (level > 4)
			for(int i=0; i<1+level/20; i++)
				PlaceRooms(FLOOR_SUPPLIES,FLOOR_CORRIDOR, 2,2, 1,1);	// Supply room

		PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(2,5),Random.Range(2,5), 1,1);	// ?
		if (level > 6)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(1,3),Random.Range(1,3), 1,1);	// ?
		if (level > 12)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(2,4),Random.Range(2,4), 1,1);	// ?
		if (level > 17)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(3,5),Random.Range(3,5), 1,1);	// ?
		if (level > 28)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(2,4),Random.Range(3,5), 1,1);	// ?
		if (level > 34)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(3,5),Random.Range(2,4), 1,1);	// ?
		if (level > 41)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(3,5),Random.Range(3,5), 1,1);	// ?
		if (level > 53)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(3,5),Random.Range(3,5), 1,1);	// ?
		if (level > 59)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(1,2),Random.Range(1,2), 1,1);	// ?
		if (level > 73)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(3,5),Random.Range(3,5), 1,1);	// ?
		if (level > 86)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(2,4),Random.Range(3,5), 1,1);	// ?
		if (level > 97)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(1,2),Random.Range(1,4), 1,1);	// ?
		if (level > 99)	PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(5,6),Random.Range(5,6), 1,1);	// ?
	
		PlaceRooms(FLOOR_LUNCH,FLOOR_CORRIDOR, lunchRoomSizeX,lunchRoomSizeY, 1,1);	// Lunch room
		if (level > 7)
			PlaceRooms(FLOOR_KITCHEN,FLOOR_LUNCH, kitchenSizeX,kitchenSizeY, 1,1);	// Kitchen
		if (level > 15)
			PlaceRooms(FLOOR_FREEZER,FLOOR_KITCHEN, 1,2, 1,1);	// Freezer

		if (level > 13)
			PlaceRooms(FLOOR_IT,FLOOR_CORRIDOR, 3,4, 1,1);	// IT Department
		if (level > 25)
			PlaceRooms(FLOOR_IT,FLOOR_CORRIDOR, 2,2, 1,1);	// IT Department
		if (level > 74)
			PlaceRooms(FLOOR_IT,FLOOR_CORRIDOR, 4,2, 1,1);	// IT Department
		if (level > 19)
			PlaceRooms(FLOOR_SERVERS,FLOOR_IT, 2,1, 1,1);	// Server room
	
		// PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, 4,3, 1,1);	// ?
		// PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, 4,3, 1,1);	// ?
		
		if (level > 2)
			for(int i=0; i<maxConferenceRows; i++)
				 PlaceRooms(FLOOR_CONFERENCE,FLOOR_CORRIDOR, 2+Random.Range(0,2),2+Random.Range(0,2), 1,maxConferencePerRow);	// Conference rooms
			 
		for(int i=0; i<maxCubicleRows; i++)
			PlaceRooms(FLOOR_CUBICLE,FLOOR_CORRIDOR, 1,2, 3,maxCubiclePerRow);	// Cubicles
		for(int i=0; i<maxToilets; i++)
			PlaceRooms(FLOOR_TOILET,FLOOR_CORRIDOR, 1,1, 2,2);	// Toilets

		if (level > 2)	PlaceRooms(FLOOR_COPY,FLOOR_CORRIDOR, 2,1, 1,1);	// Copying rooms
		if (level > 21)	PlaceRooms(FLOOR_COPY,FLOOR_CORRIDOR, 2,1, 1,1);	// Copying rooms
		if (level > 34)	PlaceRooms(FLOOR_COPY,FLOOR_CORRIDOR, 2,1, 1,1);	// Copying rooms
		if (level > 62)	PlaceRooms(FLOOR_COPY,FLOOR_CORRIDOR, 2,1, 1,1);	// Copying rooms

		for(int i=0; i<maxClosets; i++)
			PlaceRooms(FLOOR_CLOSET,FLOOR_CORRIDOR, 1,1, 1,level/35 + Random.Range(1,3));	// Closets

		for (int i=0; i<2+level/2; i++)
			PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(2,5),Random.Range(2,5), 1,1);	// Larger filler rooms?
		for (int i=0; i<2+level/2; i++)
			PlaceRooms(FLOOR_UNKNOWN,FLOOR_CORRIDOR, Random.Range(1,3),Random.Range(1,3), 1,1);	// Smaller filler rooms?
		for (int i=0; i<2+level/2; i++)
			PlaceRooms(FLOOR_UNKNOWN,FLOOR_UNKNOWN, Random.Range(1,4),Random.Range(1,4), 1,1);	// Smaller connected filler rooms?

		// Secret areas
		if (level > 3)
			for (int i=0; i<1+level/10; i++)
				if (Random.Range(0,3) == 0)
					PlaceRooms(FLOOR_SECRET,FLOOR_CORRIDOR, 1,1, 1,1);
			
		GenerateWalls();

	}

	void SetEndPoint(int in_x, int in_y, int in_dir)
	{
		scx2 = in_x;
		scy2 = in_y;
		scd2 = in_dir;
		wallMapX[in_x,in_y] = 1;
		wallMapX[in_x,in_y+1] = 1;
		wallMapY[in_x+1,in_y] = 1;
		wallMapY[in_x,in_y] = 1;
		if (in_dir == 0)	wallMapX[in_x,in_y] = 2;
		else if (in_dir == 1)	wallMapY[in_x,in_y] = 2;
		else if (in_dir == 2)	wallMapX[in_x,in_y+1] = 2;
		else if (in_dir == 3)	wallMapY[in_x+1,in_y] = 2;
		floorMap[in_x,in_y] = FLOOR_END;
		tunnelMap[in_x, in_y] = 2;
	}

	void GenerateWalls()
	{
		for(int j=0; j<levelHeight; j++)
		{
			for(int i=0; i<levelWidth; i++)
			{
				int b1 = 0, b2 = 0;
				b1 = tunnelMap[i,j];
				
				if (i < levelWidth-1)	b2 = tunnelMap[i+1,j];
				bool okToSetWall = false;
				if ((wallMapY[i+1,j] == 0) || (wallMapY[i+1,j] == 1))
				{
					if (b1 != b2)
					{
						if ((b1 >= tunnelRoomIndexStart) || (b2 >= tunnelRoomIndexStart))
							okToSetWall = true;
						else if ((b1 == 0) || (b2 == 0))
							okToSetWall = true;
					}
				}
				if (okToSetWall)	wallMapY[i+1,j] = 1;
				
				b2 = 0;
				if (j < levelHeight-1)	b2 = tunnelMap[i,j+1];
				okToSetWall = false;
				if ((wallMapX[i,j+1] == 0) || (wallMapX[i,j+1] == 1))
				{
					if (b1 != b2)
					{
						if ((b1 >= tunnelRoomIndexStart) || (b2 >= tunnelRoomIndexStart))
							okToSetWall = true;
						else if ((b1 == 0) || (b2 == 0))
							okToSetWall = true;
					}
				}
				if (okToSetWall)	wallMapX[i,j+1] = 1;
				
			}
		}
		
		for(int i = 0; i<levelHeight; i++)
			if (tunnelMap[0,i] > 0)
				if (wallMapY[0,i] != 2)
					wallMapY[0,i] = 1;
		
		for(int i = 0; i<levelWidth; i++)
			if (tunnelMap[i,0] > 0)
				if (wallMapX[i,0] != 2)
					wallMapX[i,0] = 1;
	}

	void PlaceRooms(byte in_roomType, byte in_connectToFloorType, int in_width, int in_depth, int in_minCount, int in_maxCount)
	{
		int sajsDepth = in_depth, sajsWidth = in_width, minCount = in_minCount, maxCount = in_maxCount;
		int idealCount = -1;
		
		for(int iter=0; iter < 5000; iter++)
		{
			int startX = Random.Range(0,levelWidth);
			int startY = Random.Range(0,levelHeight);
			int dir = Random.Range(0,4);
			int currentX, currentY;
			
			int stepWidthX = 0, stepWidthY = 0, stepDepthX = 0, stepDepthY = 0;
			
			if (dir == 0)	{	stepWidthX = 1;	stepWidthY = 0;	stepDepthX = 0;	stepDepthY = 1;	}
			else if (dir == 1)	{	stepWidthX = 0;	stepWidthY = 1;	stepDepthX = 1;	stepDepthY = 0;	}
			else if (dir == 2)	{	stepWidthX = -1;	stepWidthY = 0;	stepDepthX = 0;	stepDepthY = -1;	}
			else if (dir == 3)	{	stepWidthX = 0;	stepWidthY = -1;	stepDepthX = -1;	stepDepthY = 0;	}
			
			bool isOk = true;
			for(int i2 = 0; i2<maxCount; i2++)
			{
				for(int w = 0; w<sajsWidth; w++)
				{
					for(int d = 0; d<sajsDepth; d++)
					{
						currentX = startX;
						currentY = startY;
						currentX += w*stepWidthX + d*stepDepthX + i2*stepWidthX*sajsWidth;
						currentY += w*stepWidthY + d*stepDepthY + i2*stepWidthY*sajsWidth;
						
						if (!EmptySpace(currentX, currentY))
						{
							isOk = false;
							break;
						}
					}
					if (!isOk)	break;
					if (!IsCorrectFloorType(in_connectToFloorType, startX + stepDepthX*sajsDepth + w*stepWidthX + i2*stepWidthX*sajsWidth, startY + stepDepthY*sajsDepth + w*stepWidthY + i2*stepWidthY*sajsWidth))	{	isOk = false;	break;	}
				}
				
				if (isOk)
					if ((i2 > idealCount) && (i2+1 >= minCount))
						idealCount = i2+1;
					
			}
			
			if (idealCount > -1)
			{
				int doorOffset = Random.Range(0,sajsWidth);
				int boundX1 = 99999, boundY1 = 99999, boundX2 = -99999, boundY2 = -99999;
				byte doorType = 2;
				if (in_roomType == FLOOR_SECRET)	doorType = 3;
				for (int i2=0; i2<=idealCount-1; i2++)
				{
					if (in_roomType == FLOOR_SECRET)	levelSecretRoomCount++;
				
					if (dir == 0)	wallMapX[startX+doorOffset+i2*sajsWidth, startY+sajsDepth] = doorType;
					else if (dir == 1)	wallMapY[startX+sajsDepth, startY+doorOffset+i2*sajsWidth] = doorType;
					else if (dir == 2)	wallMapX[startX-doorOffset-i2*sajsWidth, startY-sajsDepth+1] = doorType;
					else if (dir == 3)	wallMapY[startX-sajsDepth+1, startY-doorOffset-i2*sajsWidth] = doorType;
					
					for(int w=0; w<sajsWidth; w++)
					{
						for(int d=0; d<sajsDepth; d++)
						{
							currentX = startX;
							currentY = startY;
							currentX += w*stepWidthX + d*stepDepthX + i2*stepWidthX*sajsWidth;
							currentY += w*stepWidthY + d*stepDepthY + i2*stepWidthY*sajsWidth;
							tunnelMap[currentX, currentY] = tunnelIndex;
							floorMap[currentX, currentY] = in_roomType;
							if (currentX < boundX1)	boundX1 = currentX;
							if (currentX > boundX2)	boundX2 = currentX;
							if (currentY < boundY1)	boundY1 = currentY;
							if (currentY > boundY2)	boundY2 = currentY;
						}
					}
					tunnelIndex++;
				}
	/*			
				// Carve out a piece of unknown rooms
				// Todo: flag door floors as non-cuttable
				if ((idealCount == 1) && (in_roomType == ROOM_UNKNOWN))
				{
					var roomWidth : int = boundX2-boundX1+1;
					var roomHeight : int = boundY2-boundY1+1;
					var carveWidth : int = Random.Range(0,roomWidth+3)-0;
					var carveHeight : int = Random.Range(0,roomHeight+3)-0;
					if ((carveWidth > 0) && (carveHeight > 0))
					{
						var carveX1 : int = boundX1 + Random.Range(0,roomWidth*2+1)-roomWidth;
						var carveY1 : int = boundY1 + Random.Range(0,roomHeight*2+1)-roomHeight;
						var carveX2 : int = carveX1+carveWidth;
						var carveY2 : int = carveY1+carveHeight;
						if ((boundX1 < carveX1) || (boundX1 > carveX2) || (boundX2 < carveX1) || (boundX2 > carveX2))
							if ((boundY1 < carveY1) || (boundY1 > carveY2) || (boundY2 < carveY1) || (boundY2 > carveY2))
								for(var cx:int=carveX1; cx <= carveX2; cx++)
									for(var cy:int=carveY1; cy <= carveY2; cy++)
										if ((cx >= 0) && (cy >= 0) && (cx < levelWidth) && (cy < levelHeight))
											if (tunnelMap[cx,cy] == tunnelIndex-1)
											{
												tunnelMap[cx,cy] = 0;
												floorMap[cx,cy] = 0;
											}
					}
					
				}
	*/			
				break;
			}
		}
		
	}
	
	bool OkToTunnel()
	{
		if (tunnelD == 0)
		{
			if (tunnelY > levelHeight-2)	return false;
			if (tunnelMap[tunnelX, tunnelY+1] > 0)	return false;
			if (tunnelX > 0)	if (tunnelMap[tunnelX-1, tunnelY+1] > 0)	return false;
			if (tunnelX < levelWidth-1)	if (tunnelMap[tunnelX+1, tunnelY+1] > 0)	return false;
			if ((tunnelX == scx1) && (tunnelY+2 == scy1))	return false;
			if ((tunnelX == scx2) && (tunnelY+2 == scy2))	return false;
		}
		else if (tunnelD == 1)
		{
			if (tunnelX > levelWidth-2)	return false;
			if (tunnelMap[tunnelX+1, tunnelY] > 0)	return false;
			if (tunnelY > 0)	if (tunnelMap[tunnelX+1, tunnelY-1] > 0)	return false;
			if (tunnelY < levelHeight-1)	if (tunnelMap[tunnelX+1, tunnelY+1] > 0)	return false;
			if ((tunnelY == scy1) && (tunnelX+2 == scx1))	return false;
			if ((tunnelY == scy2) && (tunnelX+2 == scx2))	return false;
		}
		else if (tunnelD == 2)
		{
			if (tunnelY < 1)	return false;
			if (tunnelMap[tunnelX, tunnelY-1] > 0)	return false;
			if (tunnelX > 0)	if (tunnelMap[tunnelX-1, tunnelY-1] > 0)	return false;
			if (tunnelX < levelWidth-1)	if (tunnelMap[tunnelX+1, tunnelY-1] > 0)	return false;
			if ((tunnelX == scx1) && (tunnelY-2 == scy1))	return false;
			if ((tunnelX == scx2) && (tunnelY-2 == scy2))	return false;
		}
		else if (tunnelD == 3)
		{
			if (tunnelX < 1)	return false;
			if (tunnelMap[tunnelX-1, tunnelY] > 0)	return false;
			if (tunnelY > 0)	if (tunnelMap[tunnelX-1, tunnelY-1] > 0)	return false;
			if (tunnelY < levelHeight-1)	if (tunnelMap[tunnelX-1, tunnelY+1] > 0)	return false;
			if ((tunnelY == scy1) && (tunnelX-2 == scx1))	return false;
			if ((tunnelY == scy2) && (tunnelX-2 == scx2))	return false;
		}
		return true;
	}

	void TunnelOneStep()
	{
		if (tunnelD == 0)	tunnelY++;
		else if (tunnelD == 1)	tunnelX++;
		else if (tunnelD == 2)	tunnelY--;
		else if (tunnelD == 3)	tunnelX--;
	}

	void SwitchTunnelDirection()
	{
		if ((tunnelD == 0) || (tunnelD == 2)) {	if (tunnelX == 0)	tunnelD = 1;	else if (tunnelX == levelWidth)	tunnelD = 3;	else	tunnelD = Random.Range(0,2)*2+1;	}
		else if ((tunnelD == 1) || (tunnelD == 3)) {	if (tunnelY == 0)	tunnelD = 0;	else if (tunnelY == levelHeight)	tunnelD = 2;	else	tunnelD = Random.Range(0,2)*2;	}
	}
	
	void TunnelCorridor(int in_length)
	{
		tunnelIndex++;
		
		bool tunnelAny = false;
		int l = in_length;
		for (int k=0; k<=l; k++)
		{
			if (k > 0)	{	if (OkToTunnel())	TunnelOneStep();	else break;	}
			tunnelAny = true;

			lastPathWeight++;
			if ((pathMap[tunnelX, tunnelY] > 0) && (pathMap[tunnelX, tunnelY] < lastPathWeight))
				lastPathWeight = pathMap[tunnelX, tunnelY];
			else
				pathMap[tunnelX, tunnelY] = lastPathWeight;
				
			tunnelMap[tunnelX, tunnelY] = tunnelIndex;
			floorMap[tunnelX, tunnelY] = FLOOR_CORRIDOR;
		}
		if (!tunnelAny)	tunnelIndex--;
	}

	bool IsCorrectFloorType(byte in_floorType, int in_x, int in_y)
	{
		if (in_x < 0)	return false;
		if (in_y < 0)	return false;
		if (in_x > levelWidth-1)	return false;
		if (in_y > levelHeight-1)	return false;
		if (floorMap[in_x,in_y] == in_floorType)	return true;
		return false;
	}
	
	bool EmptySpace(int in_x, int in_y)
	{
		if (in_x < 0)	return false;
		if (in_y < 0)	return false;
		if (in_x > levelWidth-1)	return false;
		if (in_y > levelHeight-1)	return false;
		if (tunnelMap[in_x,in_y] > 0)	return false;
		return true;
	}

	void SetStartPoint(int in_x, int in_y, int in_dir)
	{
		scx1 = in_x;
		scy1 = in_y;
		scd1 = in_dir;
		wallMapX[in_x,in_y] = 1;
		wallMapX[in_x,in_y+1] = 1;
		wallMapY[in_x+1,in_y] = 1;
		wallMapY[in_x,in_y] = 1;
		if (in_dir == 0)	wallMapX[in_x,in_y] = 2;
		else if (in_dir == 1)	wallMapY[in_x,in_y] = 2;
		else if (in_dir == 2)	wallMapX[in_x,in_y+1] = 2;
		else if (in_dir == 3)	wallMapY[in_x+1,in_y] = 2;
		floorMap[in_x,in_y] = FLOOR_START;
		tunnelMap[in_x, in_y] = 1;
		pathMap[in_x, in_y] = 1;
		lastPathWeight = 1;
		
//		GameObject.Find("Player").transform.position = new Vector3(scx1, 0, scy1);
		GameObject.Find("Player").transform.position = new Vector3(1, 0, 1);

	}
	
	void LoadWallPrefabs()
	{
		pfbWallX[1] = (GameObject)Resources.Load("walls/pfbWallX");
		pfbWallX[2] = (GameObject)Resources.Load("walls/pfbDoorX");
		pfbWallX[2] = (GameObject)Resources.Load("walls/pfbWallX");
		pfbWallY[1] = (GameObject)Resources.Load("walls/pfbWallY");
		pfbWallY[2] = (GameObject)Resources.Load("walls/pfbDoorY");
		pfbWallY[3] = (GameObject)Resources.Load("walls/pfbWallY");
	}
	
	void LoadFloorPrefabs()
	{
		pfbFloor[FLOOR_CORRIDOR] = (GameObject)Resources.Load("floors/pfbFloor");
		pfbFloor[FLOOR_CUBICLE] = (GameObject)Resources.Load("floors/pfbFloorCubicle");
		pfbFloor[FLOOR_LUNCH] = (GameObject)Resources.Load("floors/pfbFloorLunch");
		pfbFloor[FLOOR_KITCHEN] = (GameObject)Resources.Load("floors/pfbFloorKitchen");
		pfbFloor[FLOOR_TOILET] = (GameObject)Resources.Load("floors/pfbFloorToilet");
		pfbFloor[FLOOR_IT] = (GameObject)Resources.Load("floors/pfbFloorIT");
		pfbFloor[FLOOR_SERVERS] = (GameObject)Resources.Load("floors/pfbFloorServers");
		pfbFloor[FLOOR_CONFERENCE] = (GameObject)Resources.Load("floors/pfbFloorConference");
		pfbFloor[FLOOR_CLOSET] = (GameObject)Resources.Load("floors/pfbFloorCloset");
		pfbFloor[FLOOR_COPY] = (GameObject)Resources.Load("floors/pfbFloorCopy");
		pfbFloor[FLOOR_FREEZER] = (GameObject)Resources.Load("floors/pfbFloorFreezer");
		pfbFloor[FLOOR_MAIL] = (GameObject)Resources.Load("floors/pfbFloorMail");
		pfbFloor[FLOOR_SUPPLIES] = (GameObject)Resources.Load("floors/pfbFloorSupplies");
		pfbFloor[FLOOR_SECRET] = (GameObject)Resources.Load("floors/pfbFloorSecret");
		pfbFloor[FLOOR_START] = (GameObject)Resources.Load("floors/pfbFloorStart");
		pfbFloor[FLOOR_END] = (GameObject)Resources.Load("floors/pfbFloorEnd");
		pfbFloor[FLOOR_UNKNOWN] = (GameObject)Resources.Load("floors/pfbFloorUnknown");
	}

	void DrawMap()
	{
		for(int j=0; j<levelHeight+1; j++)
			for(int i=0; i<levelWidth+1; i++)
			{
				float i2 = i;
				float j2 = j;
				GameObject go;
				
				if ((i < levelWidth) && (j < levelHeight))
				{		
					go = pfbFloor[floorMap[i,j]];
					if (go)	Instantiate(go, new Vector3(i, 0f, j), Quaternion.identity);
				}
				
				if (wallMapX[i,j] == 1)	//	remove this condition for doors, breakable walls and windows
				{
					go = pfbWallX[wallMapX[i,j]];
					if (go)	Instantiate(go, new Vector3(i2, .5f, j2-.5f), Quaternion.identity);
				}
				
				if (wallMapY[i,j] == 1)	//	remove this condition for doors, breakable walls and windows
				{
					go = pfbWallY[wallMapY[i,j]];
					if (go)	Instantiate(go, new Vector3(i2-.5f, .5f, j2), Quaternion.identity);
				}
			}

	}
	
	void Update()
	{
	
	}
	
}
