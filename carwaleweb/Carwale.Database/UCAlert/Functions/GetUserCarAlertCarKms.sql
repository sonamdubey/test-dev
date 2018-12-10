IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[GetUserCarAlertCarKms]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [UCAlert].[GetUserCarAlertCarKms]
GO

	CREATE FUNCTION   [UCAlert].[GetUserCarAlertCarKms](@UsedCarAlertId int)
RETURNS VARCHAR(MAX)
AS
BEGIN
DECLARE     @iReturnCode       int,
            @iNextRowId        int,
            @iCurrentRowId     int,
            @iLoopControl      int,            
            @CarKm varchar(100),
            @CarKmCond  varchar(MAX) = null

 
		--SELECT @CarKm= yearid from UCAlert.UserCarAlerts where UsedCarAlert_Id=@UsedCarAlertId
		SELECT @CarKm= KmsId from UCAlert.UserCarAlerts where UsedCarAlert_Id=@UsedCarAlertId 
		
		SELECT @iLoopControl = 1
		SELECT @iNextRowId = MIN(CarKmId)
		FROM UCAlert.CarKms WHERE CarKmId in (SELECT * FROM UCAlert.Split(@CarKm))
		IF @iNextRowId IS NULL
		   BEGIN
					RETURN @CarKmCond
		   END


		SELECT   @iCurrentRowId   = CarKmId
		FROM UCAlert.CarKms WHERE CarKmId in (SELECT * FROM UCAlert.Split(@CarKm))
		and  CarKmId = @iNextRowId

		SELECT @CarKmCond= ' AND((nc.Kilometers Between ' + cast(LowerVal as varchar(100)) + ' AND '+cast(UpperVal as varchar(100))+')'
		FROM UCAlert.CarKms 
		WHERE CarKmId in (SELECT * FROM UCAlert.Split(@CarKm))
		and  CarKmId = @iNextRowId

		WHILE @iLoopControl = 1

		   BEGIN      

					SELECT  @iNextRowId = NULL 
					SELECT   @iNextRowId = MIN(CarKmId)  
					FROM     UCAlert.CarKms      
					WHERE    CarKmId > @iCurrentRowId
					and CarKmId in (SELECT * FROM UCAlert.Split(@CarKm))       

					IF ISNULL(@iNextRowId,0) = 0
					   BEGIN
								BREAK
					   END          

					SELECT  @iCurrentRowId =   CarKmId,
					        @CarKmCond=@CarKmCond+ ' OR (nc.Kilometers Between ' + cast(LowerVal as varchar(100)) + ' AND '+cast(UpperVal as varchar(100))+')'
					FROM    UCAlert.CarKms  
					WHERE   CarKmId = @iNextRowId   
					   and CarKmId in (SELECT * FROM UCAlert.Split(@CarKm))   
		               
	             			 

   END
    set @CarKmCond=@CarKmCond+')'	
    RETURN @CarKmCond
    END