IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[ConditionMonthlySummary]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CRM].[ConditionMonthlySummary]
GO

	
-- Description	:	Get Report of Monthly on the basis of Model and Make
-- Author		:	Dilip V. 04-Mar-2012
-- Modifier		:

CREATE FUNCTION [CRM].[ConditionMonthlySummary]
 (@HeadAgency BIGINT,@Agency BIGINT)  
 RETURNS varchar(100) AS  
 
	BEGIN 
	
	DECLARE	@varsql VARCHAR(100) = ''
    IF(@Agency != 0)
			BEGIN
				IF (@HeadAgency != 1)
					BEGIN
					SET @varsql +=' AND LA.Id = ' + CONVERT(CHAR(10), @Agency, 101)+ ' AND CLS.CategoryId = 3'
					END
				ELSE
					BEGIN
					SET @varsql +=' AND LA.Id = ' + CONVERT(CHAR(10), @Agency, 101)
					END
			END
		ELSE
			BEGIN
				IF(@HeadAgency != 0)
				BEGIN
					IF (@HeadAgency != 1)
						BEGIN
						SET @varsql +=' AND LA.HeadAgencyId = ' + CONVERT(CHAR(10), @HeadAgency, 101)+' AND CLS.CategoryId = 3'
						END
					ELSE
						BEGIN
						SET @varsql +=' AND LA.HeadAgencyId = ' + CONVERT(CHAR(10), @HeadAgency, 101)
						END
				END	
				
			END
	
	RETURN @varsql
	END 



