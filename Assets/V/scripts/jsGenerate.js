#pragma strict

var level : int = 0;
var levelWidth : int = 0;
var levelHeight : int = 0;
var levelSecretRoomCount : int = 0;

var ROOM_CORRIDOR : byte = 1;
var ROOM_CUBICLE : byte = 2;
var ROOM_LUNCH : byte = 3;
var ROOM_KITCHEN : byte = 4;
var ROOM_TOILET : byte = 5;
var ROOM_IT : byte = 6;
var ROOM_SERVERS : byte = 7;
var ROOM_CONFERENCE : byte = 8;
var ROOM_CLOSET : byte = 9;
var ROOM_COPY : byte = 10;
var ROOM_FREEZER : byte = 11;	// Attach to Kitchen (which is attached to Lunch Room) :D
var ROOM_MAIL : byte = 12;
var ROOM_SUPPLIES : byte = 13;
var ROOM_SECRET : byte = 14;

var ROOM_RELAX : byte = 15;
var ROOM_BAR : byte = 16;
var ROOM_CEO : byte = 17;
var ROOM_MIDDLE_MANAGEMENT : byte = 18;

var ROOM_START : byte = 252;
var ROOM_END : byte = 253;
var ROOM_UNKNOWN : byte = 254;

var MONSTER_BASIC : int = 1;

var pfbFloor : GameObject;
var pfbFloorDebug : GameObject;
var pfbWallX : GameObject;
var pfbWallY : GameObject;
var pfbDoorX : GameObject;
var pfbDoorY : GameObject;
var pfbFloorStart : GameObject;
var pfbFloorEnd : GameObject;
var pfbFloorCubicle : GameObject;
var pfbFloorToilet : GameObject;
var pfbFloorIT : GameObject;
var pfbFloorServers : GameObject;
var pfbFloorLunch : GameObject;
var pfbFloorKitchen : GameObject;
var pfbFloorConference : GameObject;
var pfbFloorCloset : GameObject;
var pfbFloorCopy : GameObject;
var pfbFloorFreezer : GameObject;
var pfbFloorMail : GameObject;
var pfbFloorSupplies : GameObject;
var pfbFloorUnknown : GameObject;
var pfbFloorSecret : GameObject;

var pfbMonster1 : GameObject;

var tunnelMap : int[,];
var floorMap : byte[,];
var wallMapX : byte[,];
var wallMapY : byte[,];
var pathMap : int[,];

private var tunnelX : int = 0;
private var tunnelY : int = 0;
private var tunnelD : int = 0;
private var oldTunnelD : int = 0;
private var tunnelIndex : int = 0;
private var scx1 : int = 0;
private var scy1 : int = 0;
private var scd1 : int = 0;
private var scx2 : int = 0;
private var scy2 : int = 0;
private var scd2 : int = 0;
private var lastPathWeight : int = 0;
private var tunnelRoomIndexStart : int = 0;

function Start()
{
	level = 2;

	// GenerateMap();
	// DrawMap();
	
//	GenerateMonsters();
}

function GenerateMap()
{
	// level parameters
	levelWidth = 16 + level / 5 + Random.Range(0,3)-1;
	levelHeight = 16 + level / 5 + Random.Range(0,3)-1;
	
	var maxCorridorTunnel1 : int = 6 + level/8;
	var maxCorridorTunnel2 : int = 19 + level/6;
	var minCorridorLength1 : int = 3 + level/50;
	var maxCorridorLength1 : int = 6 + level/30;
	var minCorridorLength2 : int = 3 + level/50;
	var maxCorridorLength2 : int = 6 + level/30;
	var maxCubiclePerRow : int = 6 + level/10;
	var maxCubicleRows : int = 5 + level/6;
	var maxConferencePerRow : int = 1 + level/15;
	var maxConferenceRows : int = 2 + level/7;
	var maxToilets : int = 1 + level/14 + Random.Range(0,2);
	var maxClosets : int = 2 + level/8 + Random.Range(0,4);
	var lunchRoomSizeX : int = 2 + level/25 + Random.Range(0,2);
	var lunchRoomSizeY : int = 2 + level/25 + Random.Range(0,2);
	var kitchenSizeX : int = 2 + level/55 + Random.Range(0,2);
	var kitchenSizeY : int = 1 + level/55 + Random.Range(0,2);
	
	// init
	floorMap = new byte[levelWidth,levelHeight];
	pathMap = new int[levelWidth,levelHeight];
	tunnelMap = new int[levelWidth,levelHeight];
	wallMapX = new byte[levelWidth+1,levelHeight+1];
	wallMapY = new byte[levelWidth+1,levelHeight+1];
	
	// clear
	for(var j:int = 0; j<levelHeight+1; j++)
		for(var i:int = 0; i<levelWidth+1; i++)
		{
			wallMapX[i,j] = 0;
			wallMapY[i,j] = 0;
		}
	for(j = 0; j<levelHeight; j++)
		for(i = 0; i<levelWidth; i++)
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
	for (var l : int = 0; l < 150000; l++)
	{
		tunnelD = Random.Range(0,4);
		if ((tunnelD == 0) && (scd1 != 2))	break;
		if ((tunnelD == 1) && (scd1 != 3))	break;
		if ((tunnelD == 2) && (scd1 != 0))	break;
		if ((tunnelD == 3) && (scd1 != 1))	break;
	}
		
	for(var ken:int = 0; ken < maxCorridorTunnel1; ken++)
	{
		TunnelCorridor(Random.Range(minCorridorLength1,maxCorridorLength1));
		if (ken < 3)	SwitchTunnelDirection();
	}

	for(ken = 0; ken < maxCorridorTunnel2; ken++)
	{
		for (l = 0; l < 5000; l++)
		{
			tunnelX = Random.Range(0,levelWidth);	// maybe limit with 1
			tunnelY = Random.Range(0,levelHeight);	// maybe limit with 1
			tunnelD = Random.Range(0,4);
			if (tunnelMap[tunnelX,tunnelY] > 2)
			{
				lastPathWeight = pathMap[tunnelX, tunnelY];
				for(var ken2:int = 0; ken2 < maxCorridorTunnel1; ken2++)
				{
					TunnelCorridor(Random.Range(minCorridorLength2,maxCorridorLength2));
					if (ken2 < 5)	SwitchTunnelDirection();
				}
				break;
			}
		}
	}

	var debugWeight : int = -1;
	var debugX : int = -1;
	var debugY : int = -1;
	for (i=0; i<levelWidth; i++)
		for (j=0; j<levelHeight; j++)
			if (pathMap[i,j] > debugWeight)
			{
				debugWeight = pathMap[i,j];
				debugX = i;
				debugY = j;
			}
	
	if (level < 100)
	{
		for(l=0; l<150000; l++)
		{
			var dirren : int = Random.Range(0,4);
			if (dirren == 0)	if (debugY < levelHeight-1)	if (tunnelMap[debugX,debugY+1] == 0)	{	SetEndPoint(debugX,debugY+1,0);	break;	}
			if (dirren == 1)	if (debugX < levelWidth-1)	if (tunnelMap[debugX+1,debugY] == 0)	{	SetEndPoint(debugX+1,debugY,1);	break;	}
			if (dirren == 2)	if (debugY > 0)	if (tunnelMap[debugX,debugY-1] == 0)	{	SetEndPoint(debugX,debugY-1,2);	break;	}
			if (dirren == 3)	if (debugX > 0)	if (tunnelMap[debugX-1,debugY] == 0)	{	SetEndPoint(debugX-1,debugY,3);	break;	}
		}
	}

	tunnelIndex++;
	tunnelRoomIndexStart = tunnelIndex;
	
	PlaceRooms(ROOM_MAIL,ROOM_CORRIDOR, 2,2, 1,1);	// Mail room

	if (level > 4)
		for(i=0; i<1+level/20; i++)
			PlaceRooms(ROOM_SUPPLIES,ROOM_CORRIDOR, 2,2, 1,1);	// Supply room

	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(2,5),Random.Range(2,5), 1,1);	// ?
	if (level > 6)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(1,3),Random.Range(1,3), 1,1);	// ?
	if (level > 12)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(2,4),Random.Range(2,4), 1,1);	// ?
	if (level > 17)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(3,5),Random.Range(3,5), 1,1);	// ?
	if (level > 28)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(2,4),Random.Range(3,5), 1,1);	// ?
	if (level > 34)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(3,5),Random.Range(2,4), 1,1);	// ?
	if (level > 41)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(3,5),Random.Range(3,5), 1,1);	// ?
	if (level > 53)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(3,5),Random.Range(3,5), 1,1);	// ?
	if (level > 59)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(1,2),Random.Range(1,2), 1,1);	// ?
	if (level > 73)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(3,5),Random.Range(3,5), 1,1);	// ?
	if (level > 86)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(2,4),Random.Range(3,5), 1,1);	// ?
	if (level > 97)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(1,2),Random.Range(1,4), 1,1);	// ?
	if (level > 99)	PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(5,6),Random.Range(5,6), 1,1);	// ?
	
	PlaceRooms(ROOM_LUNCH,ROOM_CORRIDOR, lunchRoomSizeX,lunchRoomSizeY, 1,1);	// Lunch room
	if (level > 7)
		PlaceRooms(ROOM_KITCHEN,ROOM_LUNCH, kitchenSizeX,kitchenSizeY, 1,1);	// Kitchen
	if (level > 15)
		PlaceRooms(ROOM_FREEZER,ROOM_KITCHEN, 1,2, 1,1);	// Freezer

	if (level > 13)
		PlaceRooms(ROOM_IT,ROOM_CORRIDOR, 3,4, 1,1);	// IT Department
	if (level > 25)
		PlaceRooms(ROOM_IT,ROOM_CORRIDOR, 2,2, 1,1);	// IT Department
	if (level > 74)
		PlaceRooms(ROOM_IT,ROOM_CORRIDOR, 4,2, 1,1);	// IT Department
	if (level > 19)
		PlaceRooms(ROOM_SERVERS,ROOM_IT, 2,1, 1,1);	// Server room
	
	// PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, 4,3, 1,1);	// ?
	// PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, 4,3, 1,1);	// ?
	
	if (level > 2)
		for(i=0; i<maxConferenceRows; i++)
			 PlaceRooms(ROOM_CONFERENCE,ROOM_CORRIDOR, 2+Random.Range(0,2),2+Random.Range(0,2), 1,maxConferencePerRow);	// Conference rooms
			 
	for(i=0; i<maxCubicleRows; i++)
		PlaceRooms(ROOM_CUBICLE,ROOM_CORRIDOR, 1,2, 3,maxCubiclePerRow);	// Cubicles
	for(i=0; i<maxToilets; i++)
		PlaceRooms(ROOM_TOILET,ROOM_CORRIDOR, 1,1, 2,2);	// Toilets

	if (level > 2)	PlaceRooms(ROOM_COPY,ROOM_CORRIDOR, 2,1, 1,1);	// Copying rooms
	if (level > 21)	PlaceRooms(ROOM_COPY,ROOM_CORRIDOR, 2,1, 1,1);	// Copying rooms
	if (level > 34)	PlaceRooms(ROOM_COPY,ROOM_CORRIDOR, 2,1, 1,1);	// Copying rooms
	if (level > 62)	PlaceRooms(ROOM_COPY,ROOM_CORRIDOR, 2,1, 1,1);	// Copying rooms

	for(i=0; i<maxClosets; i++)
		PlaceRooms(ROOM_CLOSET,ROOM_CORRIDOR, 1,1, 1,level/35 + Random.Range(1,3));	// Closets

	for (i=0; i<2+level/2; i++)
		PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(2,5),Random.Range(2,5), 1,1);	// Larger filler rooms?
	for (i=0; i<2+level/2; i++)
		PlaceRooms(ROOM_UNKNOWN,ROOM_CORRIDOR, Random.Range(1,3),Random.Range(1,3), 1,1);	// Smaller filler rooms?
	for (i=0; i<2+level/2; i++)
		PlaceRooms(ROOM_UNKNOWN,ROOM_UNKNOWN, Random.Range(1,4),Random.Range(1,4), 1,1);	// Smaller connected filler rooms?

	// Secret areas
	if (level > 3)
		for (i=0; i<1+level/10; i++)
			if (Random.Range(0,3) == 0)
				PlaceRooms(ROOM_SECRET,ROOM_CORRIDOR, 1,1, 1,1);
		
	GenerateWalls();
	
}

function PlaceRooms(in_roomType:byte, in_connectToFloorType:byte, in_width:int, in_depth:int, in_minCount:int, in_maxCount:int)
{
	var sajsDepth : int = in_depth;
	var sajsWidth : int = in_width;
	var minCount : int = in_minCount;
	var maxCount : int = in_maxCount;
	
	var idealCount : int = -1;
	var i2 : int = 0;
	
	for(var iter:int = 0; iter < 5000; iter++)
	{
		var startX : int = Random.Range(0,levelWidth);
		var startY : int = Random.Range(0,levelHeight);
		var dir : int = Random.Range(0,4);
		
		var stepWidthX : int = 0;
		var stepWidthY : int = 0;
		var stepDepthX : int = 0;
		var stepDepthY : int = 0;
		
		if (dir == 0)	{	stepWidthX = 1;	stepWidthY = 0;	stepDepthX = 0;	stepDepthY = 1;	}
		else if (dir == 1)	{	stepWidthX = 0;	stepWidthY = 1;	stepDepthX = 1;	stepDepthY = 0;	}
		else if (dir == 2)	{	stepWidthX = -1;	stepWidthY = 0;	stepDepthX = 0;	stepDepthY = -1;	}
		else if (dir == 3)	{	stepWidthX = 0;	stepWidthY = -1;	stepDepthX = -1;	stepDepthY = 0;	}
		
		var isOk : boolean = true;
		for(i2 = 0; i2<maxCount; i2++)
		{
			for(var w:int = 0; w<sajsWidth; w++)
			{
				for(var d:int = 0; d<sajsDepth; d++)
				{
					var currentX : int = startX;
					var currentY : int = startY;
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
			var doorOffset : int = Random.Range(0,sajsWidth);
			var boundX1 : int = 99999;
			var boundY1 : int = 99999;
			var boundX2 : int = -99999;
			var boundY2 : int = -99999;
			var doorType : byte = 2;
			if (in_roomType == ROOM_SECRET)	doorType = 3;
			for (i2=0; i2<=idealCount-1; i2++)
			{
				if (in_roomType == ROOM_SECRET)	levelSecretRoomCount++;
			
				if (dir == 0)	wallMapX[startX+doorOffset+i2*sajsWidth, startY+sajsDepth] = doorType;
				else if (dir == 1)	wallMapY[startX+sajsDepth, startY+doorOffset+i2*sajsWidth] = doorType;
				else if (dir == 2)	wallMapX[startX-doorOffset-i2*sajsWidth, startY-sajsDepth+1] = doorType;
				else if (dir == 3)	wallMapY[startX-sajsDepth+1, startY-doorOffset-i2*sajsWidth] = doorType;
				
				for(w = 0; w<sajsWidth; w++)
				{
					for(d = 0; d<sajsDepth; d++)
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
						if (currentY > boundY1)	boundY2 = currentY;
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

function GenerateMonsters()
{
	for(var i:int = 0; i<7+level/2; i++)
		PlaceMonster(pfbMonster1, 1, ROOM_CUBICLE);
	for(i = 0; i<12+level; i++)
		PlaceMonster(pfbMonster1, 1, ROOM_CORRIDOR);
	for(i = 0; i<4+level/2; i++)
		PlaceMonster(pfbMonster1, 1, ROOM_LUNCH);
}

function PlaceMonster(in_type:GameObject, in_count:int, in_floorType:byte)
{
	for (var iter:int = 0; iter < 1000; iter++)
	{
		var x:int = Random.Range(0, levelWidth);
		var y:int = Random.Range(0, levelHeight);
		var xf:float = x;
		var yf:float = y;
		if (floorMap[x,y] == in_floorType)
		{
			Instantiate(in_type, new Vector3(xf+Random.Range(.125f,.875f)-.5f, 0f, yf+Random.Range(.125f,.875f)-.5f), Quaternion.identity);
			break;
		}
	}
}

function EmptySpace(in_x:int, in_y:int):boolean
{
	if (in_x < 0)	return false;
	if (in_y < 0)	return false;
	if (in_x > levelWidth-1)	return false;
	if (in_y > levelHeight-1)	return false;
	if (tunnelMap[in_x,in_y] > 0)	return false;
	return true;
}

function IsCorrectFloorType(in_floorType:byte, in_x:int, in_y:int):boolean
{
	if (in_x < 0)	return false;
	if (in_y < 0)	return false;
	if (in_x > levelWidth-1)	return false;
	if (in_y > levelHeight-1)	return false;
	if (floorMap[in_x,in_y] == in_floorType)	return true;
	return false;
}

function SetEndPoint(in_x:int, in_y:int, in_dir:int)
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
	floorMap[in_x,in_y] = ROOM_END;
	tunnelMap[in_x, in_y] = 2;
}

function SetStartPoint(in_x:int, in_y:int, in_dir:int)
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
	floorMap[in_x,in_y] = ROOM_START;
	tunnelMap[in_x, in_y] = 1;
	pathMap[in_x, in_y] = 1;
	lastPathWeight = 1;
}

function TunnelCorridor(in_length:int)
{
	tunnelIndex++;
	
	var tunnelAny : boolean = false;
	var l : int = in_length;
	for (var k:int = 0; k<=l; k++)
	{
		if (k > 0)	{	if (OkToTunnel())	TunnelOneStep();	else break;	}
		tunnelAny = true;

		lastPathWeight++;
		if ((pathMap[tunnelX, tunnelY] > 0) && (pathMap[tunnelX, tunnelY] < lastPathWeight))
			lastPathWeight = pathMap[tunnelX, tunnelY];
		else
			pathMap[tunnelX, tunnelY] = lastPathWeight;
			
		tunnelMap[tunnelX, tunnelY] = tunnelIndex;
		floorMap[tunnelX, tunnelY] = ROOM_CORRIDOR;
	}
	if (!tunnelAny)	tunnelIndex--;
}

function GenerateWalls()
{
	for(var j:int=0; j<levelHeight; j++)
	{
		for(var i:int=0; i<levelWidth; i++)
		{
			var b1 : int = 0;
			var b2 : int = 0;
			b1 = tunnelMap[i,j];
			
			if (i < levelWidth-1)	b2 = tunnelMap[i+1,j];
			var okToSetWall : boolean = false;
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
	
	for(i = 0; i<levelHeight; i++)
		if (tunnelMap[0,i] > 0)
			if (wallMapY[0,i] != 2)
				wallMapY[0,i] = 1;
	
	for(i = 0; i<levelWidth; i++)
		if (tunnelMap[i,0] > 0)
			if (wallMapX[i,0] != 2)
				wallMapX[i,0] = 1;
	
	Debug.Log(tunnelRoomIndexStart);
}

function SwitchTunnelDirection()
{
	if ((tunnelD == 0) || (tunnelD == 2)) {	if (tunnelX == 0)	tunnelD = 1;	else if (tunnelX == levelWidth)	tunnelD = 3;	else	tunnelD = Random.Range(0,2)*2+1;	}
	else if ((tunnelD == 1) || (tunnelD == 3)) {	if (tunnelY == 0)	tunnelD = 0;	else if (tunnelY == levelHeight)	tunnelD = 2;	else	tunnelD = Random.Range(0,2)*2;	}
	/*
	if ((tunnelD == 0) || (tunnelD == 2))	tunnelD = Random.Range(0,2)*2+1;
	else tunnelD = Random.Range(0,2)*2;
	*/
}

function OkToTunnel():boolean
{
	var i:int = 0;
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

function TunnelOneStep()
{
	if (tunnelD == 0)	tunnelY++;
	else if (tunnelD == 1)	tunnelX++;
	else if (tunnelD == 2)	tunnelY--;
	else if (tunnelD == 3)	tunnelX--;
}

function DrawMap()
{
	var i:int = 0;
	var j:int = 0;
	for(j = 0; j<levelHeight+1; j++)
		for(i = 0; i<levelWidth+1; i++)
		{
			var i2 : float = i;
			var j2 : float = j;
			
			if ((i < levelWidth) && (j < levelHeight))
			{		
				var go : GameObject;
				
				if (floorMap[i,j] == ROOM_CORRIDOR)	go = Instantiate(pfbFloor, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_START)	go = Instantiate(pfbFloorStart, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_END)	go = Instantiate(pfbFloorEnd, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_CUBICLE)	go = Instantiate(pfbFloorCubicle, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_TOILET)	go = Instantiate(pfbFloorToilet, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_IT)	go = Instantiate(pfbFloorIT, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_SERVERS)	go = Instantiate(pfbFloorServers, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_LUNCH)	go = Instantiate(pfbFloorLunch, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_KITCHEN)	go = Instantiate(pfbFloorKitchen, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_CLOSET)	go = Instantiate(pfbFloorCloset, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_CONFERENCE)	go = Instantiate(pfbFloorConference, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_COPY)	go = Instantiate(pfbFloorCopy, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_FREEZER)	go = Instantiate(pfbFloorFreezer, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_MAIL)	go = Instantiate(pfbFloorMail, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_SUPPLIES)	go = Instantiate(pfbFloorSupplies, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_SECRET)	go = Instantiate(pfbFloorSecret, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] == ROOM_UNKNOWN)	go = Instantiate(pfbFloorUnknown, new Vector3(i, -.5f, j), Quaternion.identity);
				else if (floorMap[i,j] > 0)	go = Instantiate(pfbFloorDebug, new Vector3(i, -.5f, j), Quaternion.identity);
				
				if (go)	go.GetComponent(jsMouseOver).b = tunnelMap[i,j].ToString();
			}
			
			if (wallMapX[i,j] > 0)
			{
				if (wallMapX[i,j] == 1)
					Instantiate(pfbWallX, new Vector3(i2, 0, j2-.5f), Quaternion.identity);
				else if (wallMapX[i,j] == 2)
					Instantiate(pfbDoorX, new Vector3(i2, 0, j2-.5f), Quaternion.identity);
				else if (wallMapX[i,j] == 3)
					Instantiate(pfbWallX, new Vector3(i2, 0, j2-.5f), Quaternion.identity);
			}
			if (wallMapY[i,j] > 0)
			{
				if (wallMapY[i,j] == 1)
					Instantiate(pfbWallY, new Vector3(i2-.5f, 0, j2), Quaternion.identity);
				else if (wallMapY[i,j] == 2)
					Instantiate(pfbDoorY, new Vector3(i2-.5f, 0, j2), Quaternion.identity);
				else if (wallMapY[i,j] == 3)
					Instantiate(pfbWallY, new Vector3(i2-.5f, 0, j2), Quaternion.identity);
			}
		}

}
