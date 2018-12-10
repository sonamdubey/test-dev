IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ClearCustStockPhotoLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ClearCustStockPhotoLog]
GO

	-- =============================================
-- Author:		Garule Prabhudas
-- Create date: 26th oct,2016
-- Description:	Clear CustStockPhotolog table after pusing images corresponding to inquiryIds to CT
-- =============================================
CREATE PROCEDURE [dbo].[ClearCustStockPhotoLog]
	@MaxId INT,
	@CustStockPhotoLogIdList VARCHAR(8000),
	@Delimeter VARCHAR(1)
AS
BEGIN
	
	SET NOCOUNT ON;
	--Delete logs fetched in last batch job except those which failed due to network error/ct db failure
	DELETE FROM  CustStockPhotoLog WHERE Id <= @MaxId AND Id NOT IN (SELECT items FROM [dbo].[SplitText](@CustStockPhotoLogIdList,@Delimeter))
END

