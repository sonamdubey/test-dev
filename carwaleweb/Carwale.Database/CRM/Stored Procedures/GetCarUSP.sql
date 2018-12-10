IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetCarUSP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetCarUSP]
GO

	



--Summary	: THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR CarUSP
--Author	: Dilip V. 31-Jul-2012

CREATE PROCEDURE [CRM].[GetCarUSP]
	@Id NUMERIC = NULL
 AS
	
BEGIN
	SET NOCOUNT ON	
	IF @Id IS NULL
		BEGIN
			SELECT CCU.id,CMA.Name CarMake,CMO.Name CarModel,IsActive,Title,USPDescription 
			FROM CRM.CarUSP	CCU
			INNER JOIN CarModels CMO ON CMO.ID = CCU.ModelId
			INNER JOIN CarMakes CMA ON CMA.ID = CCU.MakeId
		END
	ELSE
		BEGIN
			SELECT CCU.id,CMA.Name CarMake,CMO.Name CarModel,IsActive,Title,USPDescription 
			FROM CRM.CarUSP	CCU
			INNER JOIN CarModels CMO ON CMO.ID = CCU.ModelId
			INNER JOIN CarMakes CMA ON CMA.ID = CCU.MakeId
			WHERE CCU.id = @Id
		
			
		END
	
		
END




