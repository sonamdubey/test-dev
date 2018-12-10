IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_DeleteDealeRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_DeleteDealeRules]
GO

	-- =============================================
-- Author	:	Sachin Bharti(15th July 2014)
-- Description	:	Make Inactive NCD_Dealers defined rule from NCD_DefinedDealerRules table
-- =============================================
CREATE PROCEDURE [dbo].[NCD_DeleteDealeRules]  
	@NCD_DealerRuleIds	VARCHAR(100),
	@IsDeleted	SMALLINT = NULL OUTPUT 
AS
BEGIN
	
	SET NOCOUNT ON;
	SET @IsDeleted = -1
	UPDATE  NCD_DefinedDealerRules SET IsActive = 0
	WHERE ID IN (select listmember from fnSplitCSV(@NCD_DealerRuleIds))
	IF @@ROWCOUNT <> 0
	BEGIN
		SET @IsDeleted = 1
	END
END
