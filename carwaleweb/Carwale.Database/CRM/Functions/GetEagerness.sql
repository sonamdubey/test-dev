IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetEagerness]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CRM].[GetEagerness]
GO

	CREATE FUNCTION [CRM].[GetEagerness]
 (@Eagerness	SMALLINT)  
 RETURNS varchar(100) AS  
	BEGIN  
	DECLARE	@varsql VARCHAR(100)
    IF(@Eagerness = 1)--Hot
		SET @varsql = ' AND CII.ClosingProbability IN (1,2) AND CII.ProductTypeId = 1'
	ELSE IF(@Eagerness = 2)--Normal
		SET @varsql = ' AND CII.ClosingProbability = 3 AND CII.ProductTypeId = 1'
	ELSE IF(@Eagerness = 3)--Cold
		SET @varsql = ' AND CII.ClosingProbability = 4 AND CII.ProductTypeId = 1'
	ELSE IF(@Eagerness = 4)--Other
		SET @varsql = ' AND CII.ClosingProbability = -1 AND CII.ProductTypeId = 1'
	
	Return @varsql
	END 