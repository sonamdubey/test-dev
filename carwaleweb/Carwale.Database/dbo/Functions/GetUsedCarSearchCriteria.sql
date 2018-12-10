IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarSearchCriteria]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetUsedCarSearchCriteria]
GO

	-- =============================================
-- Author:		SURENDRA CHOUKSEY ON 7TH jUNE
-- Description:	RETURN USED CAR SEARCH STRING
-- select dbo.GetUsedCarSearchCriteria(1,null,null,null,null,1,2,null,null,null,0)
 --declare @dt1 date= GETDATE(),@dt2 date=GETDATE()-760 select dbo.GetUsedCarSearchCriteria(1,0,100000,@dt2,@dt1,null,null,'236','1,2','1,2',1)
-- Modified By : Tejashree Patil on 27 July 2012 : Checked condition for loose inq 
-- Modified By : Tejashree Patil Condition for MakeYear
-- =============================================
CREATE FUNCTION [dbo].[GetUsedCarSearchCriteria]
(
@CityId SMALLINT,
@FromPrice BIGINT=NULL,
@ToPrice BIGINT=NULL,
@FromYear DATE=NULL,
@ToYear DATE=NULL,
@MakeId INT=NULL,
@ModelId INT=NULL,
@ModelIds VARCHAR(200)=NULL, -- IN CASE OF LOOSE INQUIRY COMMA SEPARATED MODELIDS WILL GET
@FuelType VARCHAR(100)=NULL,
@BodyType VARCHAR(100)=NULL,
@IsLooseMatch BIT --TO IDENTIFY LOOSE INQUIRY OR NOT
)
RETURNS VARCHAR(500)
AS
BEGIN

	DECLARE @RetVal VARCHAR(500)=''
			
	-- FOR CITY ID
	SET @RetVal+='dist=0&seller=1&city='+ CAST(@CityId AS VARCHAR(100))
	
	-- MATCH LEADS FROM STOCK OR BUYER WITH STOCK OR SELLER INQUIRY
	IF(@IsLooseMatch<>1)
		BEGIN
			SET @RetVal+='&make=' + CAST(@MakeId AS VARCHAR)+ '&model=' + CAST(@MakeId AS VARCHAR)  + '.' +  CAST(@ModelId AS VARCHAR)
		END
	ELSE	-- MATCH LEADS FROM LOOSE INQUIRY
		BEGIN			
			--FOR YEAR
			-- Modified By: Tejashree Patil
			DECLARE @RetYear VARCHAR(100)='',@CountYear VARCHAR(5)
			DECLARE @FYear VARCHAR(5)=DATEPART(YY,@FromYear)
			DECLARE @TYear VARCHAR(5)=DATEPART(YY,@ToYear)
			DECLARE @CurrentYear VARCHAR(5)=DATEPART(YYYY,GETDATE())
			IF(@FromYear IS NOT NULL AND @ToYear IS NOT NULL)
			BEGIN				
				IF ((@TYear > @CurrentYear-1) OR (@FYear = @CurrentYear))
				BEGIN
					SET @CountYear=',0'
					SET @RetYear+=@CountYear
				END
				
				IF((@TYear BETWEEN @CurrentYear-2 AND @CurrentYear-1 ) OR (@FYear BETWEEN @CurrentYear-3 AND @CurrentYear-2) )
				BEGIN
					SET @CountYear=',1'
					SET @RetYear+=@CountYear
				END
				
				IF((@TYear BETWEEN @CurrentYear-4 AND @CurrentYear-3) OR (@FYear BETWEEN @CurrentYear-5 AND @CurrentYear-4) )
				BEGIN
					SET @CountYear=',2'
					SET @RetYear+=@CountYear
				END
				
				IF((@TYear BETWEEN @CurrentYear-7 AND @CurrentYear-5) OR (@FYear BETWEEN @CurrentYear-8 AND @CurrentYear-6) )
				BEGIN
					SET @CountYear=',3'
					SET @RetYear+=@CountYear
				END
				
				IF(( @TYear<= @CurrentYear-8 ) OR (@FYear < @CurrentYear-8) )
				BEGIN
					SET @CountYear=',4'
					SET @RetYear+=@CountYear
				END			
				
			END
	
			-- IF MORE THAN ONE VALUE OF YEAR
			IF(LEN(@RetYear)>2)
				BEGIN
					DECLARE @StartY VARCHAR=CAST(SUBSTRING(@RetYear,2,1) as TINYINT),@EndY VARCHAR=CAST(SUBSTRING(@RetYear,4,1)AS TINYINT)
					WHILE(@StartY <=@EndY)
					BEGIN
						SET @RetVal+='&year='+@StartY
						SET @StartY+=1
					END
				END
			ELSE --IF ONLY ONE VALUE OF YEAR
				BEGIN
					SET @RetVal+= REPLACE(@RetYear,',','&year=')
				END					
								
			--FOR PRICE
			DECLARE @RetPrice VARCHAR(100)='',@Count VARCHAR(5)
			IF(@FromPrice IS NOT NULL AND @ToPrice IS NOT NULL)
				BEGIN	
					IF((@FromPrice BETWEEN 0 AND 99999) OR (@ToPrice < 100001) )
					BEGIN
						SET @Count=',0'
						SET @RetPrice+=@Count
					END
					IF((@FromPrice BETWEEN 100000 AND 299999) OR (@ToPrice BETWEEN 100001 AND 300000) )
					BEGIN
						SET @Count=',1'
						SET @RetPrice+=@Count
					END
					IF((@FromPrice BETWEEN 300000 AND 499999) OR (@ToPrice BETWEEN 300001 AND 500000) )
					BEGIN
						SET @Count=',2'
						SET @RetPrice+=@Count
					END
					IF((@FromPrice BETWEEN 500000 AND 799999) OR (@ToPrice BETWEEN 500001 AND 800000) )
					BEGIN
						SET @Count=',3'
						SET @RetPrice+=@Count
					END
					IF((@FromPrice BETWEEN 800000 AND 1399999) OR (@ToPrice BETWEEN 800001 AND 1300000) )
					BEGIN
						SET @Count=',4'
						SET @RetPrice+=@Count
					END
					IF((@FromPrice BETWEEN 1300000 AND 1999999) OR (@ToPrice BETWEEN 1300001 AND 2000000) )
					BEGIN
						SET @Count=',5'
						SET @RetPrice+=@Count
					END
					IF((@FromPrice > 2000000) OR (@ToPrice >2000000) )
					BEGIN
						SET @Count=',6'
						SET @RetPrice+=@Count
					END
					-- IF MORE THAN ONE VALUE OF BUDGET
					IF(LEN(@RetPrice)>2)
						BEGIN
							DECLARE @Start VARCHAR=CAST(SUBSTRING(@RetPrice,2,1) as TINYINT),@End VARCHAR=CAST(SUBSTRING(@RetPrice,4,1)AS TINYINT)
							WHILE(@Start <=@End)
							BEGIN
								SET @RetVal+='&budget='+@Start
								SET @Start+=1
							END
						END
					ELSE --IF ONLY ONE VALUE OF BUDGET
						BEGIN
							SET @RetVal+= REPLACE(@RetPrice,',','&budget=')
						END
				END		
			--FOR MAKE AND MODEL IDS IN CASE OF LOOSE INQUIRY
			IF(@ModelIds IS NOT NULL)
			BEGIN
				DECLARE @MakeIds VARCHAR(200),@Makes VARCHAR(100)
				SELECT @Makes =COALESCE(@Makes+'&make=' ,'') + convert(VARCHAR,V.CarMakeId),
				@MakeIds =COALESCE(@MakeIds+'&model=' ,'') + (convert(VARCHAR,V.CarMakeId) + '.' + convert(VARCHAR,V.ID))				
				FROM CarModels V WHERE V.ID in(select listmember from [dbo].[fnSplitCSV](@ModelIds)) 
				SET @MakeIds='&model=' + @MakeIds
				SET @Makes='&make=' + @Makes				
				SET @RetVal+=@MakeIds + @Makes
			END
			
			--FOR FUEL TYPE
			IF(@FuelType IS NOT NULL)
			BEGIN
				SET @RetVal+='&fuel=' + REPLACE(@FuelType,',','&fuel=')
			END
			
			--FOR BODY TYPE
			IF(@BodyType IS NOT NULL)
			BEGIN
				SET @RetVal+='&bs=' + REPLACE(@BodyType,',','&bs=')
			END
			
		END

	RETURN @RetVal
END


