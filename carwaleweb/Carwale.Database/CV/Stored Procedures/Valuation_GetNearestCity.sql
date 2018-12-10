IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CV].[Valuation_GetNearestCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CV].[Valuation_GetNearestCity]
GO

	-- =============================================    
-- Author:  Umesh Ojha    
-- Create date: 18/2/2013    
-- Description: Gettting Nearest Valuation City  
-- =============================================    
CREATE PROCEDURE [CV].[Valuation_GetNearestCity]     
 -- Add the parameters for the stored procedure here    
 @CityId INT 
    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;  
    
	DECLARE @CityNameID  VARCHAR(70)
	DECLARE @ConstLt FLOAT = 0.030694236 
	DECLARE @ConstLg FLOAT= 0.028870889 -- // constants 
	DECLARE @Lg FLOAT 
	DECLARE @Lt FLOAT
	DECLARE @Lattitude FLOAT
	DECLARE @Longitude FLOAT
	DECLARE @CurrentDistance FLOAT =0 
	DECLARE @NearestCityId SMALLINT 
	DECLARE @MinimumDistance FLOAT= 0 
	DECLARE @LtValuation FLOAT = 0.0 
	DECLARE @LgValuation FLOAT = 0.0 
	DECLARE @TblCity TABLE 
	( 
		Id        INT IDENTITY(1, 1), 
		CityId    SMALLINT, 
		CityName  VARCHAR(50), 
		Lattitude DECIMAL(18, 4), 
		Longitude DECIMAL(18, 4) 
	) 
	DECLARE @WhileLoopControl SMALLINT=1 
	DECLARE @TotalLoopCount SMALLINT 

	SELECT @Lattitude = lattitude, 
		 @Longitude = longitude 
	FROM   Cities 
	WHERE  Id = @CityId 

	IF ( @Lattitude IS NOT NULL 
		OR @Longitude IS NOT NULL ) 
	BEGIN 
		SET @Lg = ISNULL(@Longitude,0) 
		SET @Lt = ISNULL(@Lattitude,0) 
	--print  @lt 
	--print  @lg 
	END 

	INSERT INTO @TblCity 
			  (Cityid, 
			   Cityname, 
			   Lattitude, 
			   Longitude) 
	SELECT Ci.Id, 
		 Ci.Name, 
		 Ci.Lattitude, 
		 Ci.Longitude 
	FROM   Cities Ci 
		 JOIN CarvaluesCityDeviation CD 
		   ON Ci.Id = CD.CityId 

	SELECT @TotalLoopCount = Count(*) 
	FROM   @TblCity 

	WHILE @WhileLoopControl <= @TotalLoopCount 
		BEGIN 
			SELECT @LtValuation = Lattitude, 
			   @LgValuation = Longitude, 
			   @CityId = Cityid 
			FROM   @TblCity 
			WHERE  Id = @WhileLoopControl 

			--  print  'lattitude ' + convert(varchar ,@ltValuation) + '  longitude ' + convert(varchar,@lgValuation)
			--PRINT 'compare city ' +convert(varchar,@cityid) 
			SET @CurrentDistance= Sqrt( Power( ((@Lt - @LtValuation) * @ConstLt), 
										   2 
										  ) 
									 + Power( (( @Lg - @LgValuation) * @ConstLg ), 
											2)
								   ) 

			--SET @currentDistance=Sqrt( ((@lt - @ltValuation)*@constLt) * ((@lt - @ltValuation)*@constLt) + ((@lg - @lgValuation)*@constLg) * ((@lg - @lgValuation)*@constLg) ); 
			--print power(((@lt-@ltValuation)*@constLt),2) 
			--print power(((@lg-@lgValuation)*@constLg),2) 
			--PRINT power(((@lt-@ltValuation)*@constLt),2)+power(((@lg-@lgValuation)*@constLg),2) 
			--print @constLt 
			--print (@lt-@ltValuation)*@constLt 
			--print  (@lg-@lgValuation) 
			--print (@lg-@lgValuation)*@constLg 
			--print 'Current distance  '+convert(varchar,@currentDistance) 
			IF @CurrentDistance < @MinimumDistance OR @WhileLoopControl = 1 
				SET @MinimumDistance=@CurrentDistance 

			IF ( Round(@CurrentDistance, 2) <= Round(@MinimumDistance, 2) ) 
			--// is first step or current distance is less than minimum. 
				BEGIN 
				  SET @MinimumDistance = @CurrentDistance 
				  SET @NearestCityId = @CityId 
				--           PRINT  ' CITY ID ' + CONVERT(VARCHAR,@currentDistance) +'  CURRENT DIST ' + CONVERT(VARCHAR,@CURRENTDISTANCE) 
				END 

			SET @whileloopcontrol= @whileloopcontrol + 1 
        END 

        SELECT @CityNameID = Convert(VARCHAR,ID)+':'+ Name FROM Cities WHERE Id=@NearestCityId  
        select @CityNameID AS CityNameId
      --print  'nearest city'   
 
END  
  
  