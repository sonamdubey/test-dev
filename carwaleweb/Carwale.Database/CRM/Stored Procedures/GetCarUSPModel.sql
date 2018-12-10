IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetCarUSPModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetCarUSPModel]
GO

	
--Summary	: SELECE Active Model of CarUSP
--Author	: Dilip V. 01-Aug-2012

CREATE PROCEDURE [CRM].[GetCarUSPModel]
	@MakeId NUMERIC
 AS
	
BEGIN
	SET NOCOUNT ON	
		
		SELECT DISTINCT CMO.ID AS Value,CMO.Name AS Text
		FROM CRM.CarUSP	CCU
		INNER JOIN CarModels CMO ON CMO.ID = CCU.ModelId
		WHERE CCU.IsActive = 1 AND CCU.MakeId = @MakeId
		ORDER BY CMO.Name
END



