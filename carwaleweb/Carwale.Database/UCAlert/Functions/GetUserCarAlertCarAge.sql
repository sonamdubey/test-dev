IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[GetUserCarAlertCarAge]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [UCAlert].[GetUserCarAlertCarAge]
GO

	-- select  [UCAlert].[GetUserCarAlertCarAge_bak](5)
CREATE FUNCTION   [UCAlert].[GetUserCarAlertCarAge](@UsedCarAlertId int)
RETURNS VARCHAR(MAX)
AS
BEGIN
DECLARE     @iReturnCode       int,
            @iNextRowId        int,
            @iCurrentRowId     int,
            @iLoopControl      int,            
            @CarAge varchar(100),
            @CarAgeCond  varchar(MAX) = null

 
		SELECT @CarAge= yearid from UCAlert.UserCarAlerts where UsedCarAlert_Id=@UsedCarAlertId
		
		SELECT @iLoopControl = 1
		SELECT @iNextRowId = MIN(CarAgeId)
		FROM UCAlert.CarAge WHERE CarAgeId in (SELECT * FROM UCAlert.Split(@CarAge))
		IF @iNextRowId IS NULL
		   BEGIN
					RETURN @CarAgeCond
		   END


		SELECT   @iCurrentRowId   = CarAgeId
		FROM UCAlert.CarAge WHERE CarAgeId in (SELECT * FROM UCAlert.Split(@CarAge))
		and  CarAgeId = @iNextRowId

		SELECT @CarAgeCond= ' AND((nc.MakeYear Between '  + CAST(YEAR(CAST(DATEADD(YEAR,-UpperVal,GETDATE()) AS VARCHAR(100))) AS CHAR(4)) + ' AND '+CAST(YEAR(CAST(DATEADD(YEAR,-LowerVal,GETDATE()) AS VARCHAR(100))) AS CHAR(4))+')'
		FROM UCAlert.CarAge 
		WHERE CarAgeId in (SELECT * FROM UCAlert.Split(@CarAge))
		and  CarAgeId = @iNextRowId

		WHILE @iLoopControl = 1

		   BEGIN      

					SELECT  @iNextRowId = NULL 
					SELECT   @iNextRowId = MIN(CarAgeId)  
					FROM     UCAlert.CarAge      
					WHERE    CarAgeId > @iCurrentRowId
					and CarAgeId in (SELECT * FROM UCAlert.Split(@CarAge))       

					IF ISNULL(@iNextRowId,0) = 0
					   BEGIN
								BREAK
					   END          

					SELECT  @iCurrentRowId =   CarAgeId,
					        @CarAgeCond=@CarAgeCond+ ' OR (nc.MakeYear Between '  + CAST(YEAR(CAST(DATEADD(YEAR,-UpperVal,GETDATE()) AS VARCHAR(100))) AS CHAR(4)) + ' AND '+CAST(YEAR(CAST(DATEADD(YEAR,-LowerVal,GETDATE()) AS VARCHAR(100))) AS CHAR(4))+')'
					FROM    UCAlert.CarAge  
					WHERE   CarAgeId = @iNextRowId   
					   and CarAgeId in (SELECT * FROM UCAlert.Split(@CarAge))   
		               
	             			 

   END
    set @CarAgeCond=@CarAgeCond+')'	
    RETURN @CarAgeCond
    END