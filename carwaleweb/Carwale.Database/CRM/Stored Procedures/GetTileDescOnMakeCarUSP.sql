IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetTileDescOnMakeCarUSP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetTileDescOnMakeCarUSP]
GO

	



--Summary	: THIS PROCEDURE Select RECORDS From CarUSP
--Author	: Dilip V. 31-Jul-2012

CREATE PROCEDURE [CRM].[GetTileDescOnMakeCarUSP]		
	@ModelId			NUMERIC
 AS
	
BEGIN
	SET NOCOUNT ON
		SELECT Title, USPDescription FROM CRM.CarUSP WHERE Modelid = @ModelId
END


