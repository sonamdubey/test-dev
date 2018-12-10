IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarModels]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 09-07-2012
-- Description:	Return Car Model details
-- =============================================
CREATE PROCEDURE [cw].[GetCarModels]
	-- Add the parameters for the stored procedure here
	@MakeId SMALLINT,
	@ModelCond varchar(10),
	@carYear smallint = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (@ModelCond='Upcoming')	
	SELECT ID AS Value, Name AS Text 
    FROM CarModels 
    WHERE IsDeleted = 0 
    AND CarMakeId = @MakeId 
    AND Futuristic=1     
    ORDER BY Text
    
    ELSE
    IF (@ModelCond='Sellcar')	
	SELECT Id AS Value,Name AS Text 
	FROM CarModels 
	WHERE IsDeleted = 0 AND Futuristic = 0 
	AND Used = 1 AND CarMakeId = @MakeId 
	ORDER BY Name
	
	ELSE
    IF (@ModelCond='NewModels')	
	SELECT ID AS Value, Name AS Text 
	FROM CarModels 
	WHERE IsDeleted = 0 
	AND Futuristic = 0 
	AND  CarMakeId =@MakeId 
	AND New = 1 
	ORDER BY Text  
	
	ELSE
    IF (@ModelCond='PQ')	
	SELECT DISTINCT Mo.Name AS Text, Mo.ID Value 
	FROM CarModels Mo, CarVersions Vs, Con_NewCarNationalPrices Ncp 
	WHERE Mo.IsDeleted = 0 AND Mo.CarMakeId = @MakeId 
	AND Mo.ID = Vs.CarModelId 
	AND Vs.IsDeleted = 0 
	AND Vs.New = 1 
	AND Vs.ID = Ncp.VersionId 
	AND Ncp.IsActive = 1 
	ORDER BY Text
	
	ELSE
    IF (@ModelCond='Valuation')	
	SELECT DISTINCT Mo.ID AS Value, Mo.Name AS Text
    FROM CarModels Mo, CarVersions Ve
    WHERE Ve.IsDeleted = 0 AND Mo.Id=Ve.CarModelId 
    AND Ve.Id IN ( SELECT CarVersionId FROM CarValues WHERE CarYear=@carYear ) 
    AND CarMakeId=@makeId 
    ORDER BY Text



END
