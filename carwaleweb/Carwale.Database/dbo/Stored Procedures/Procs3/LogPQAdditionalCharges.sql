IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LogPQAdditionalCharges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LogPQAdditionalCharges]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 27/06/2016
-- Description:	Log the changes done in pq charges (PQ_category)
-- =============================================
CREATE PROCEDURE [dbo].[LogPQAdditionalCharges] @ItemCategoryId INT
	,@Changes VARCHAR(max)
	,@LogMessage VARCHAR(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO PQ_CategoryItems_log (
		ItemCategoryId
		,CategoryId
		,CategoryName
		,Type
		,Scope
		,UpdatedBy
		,UpdatedOn
		,IsActive
		,Changes
		,LogMessage
		)
	SELECT Id
		,CategoryId
		,CategoryName
		,Type
		,Scope
		,UpdatedBy
		,UpdatedOn
		,IsActive
		,@Changes
		,@LogMessage
	FROM PQ_CategoryItems WITH (NOLOCK)
	WHERE id = @ItemCategoryId
END

