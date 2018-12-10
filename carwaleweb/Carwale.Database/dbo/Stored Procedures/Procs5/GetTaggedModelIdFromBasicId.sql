IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTaggedModelIdFromBasicId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTaggedModelIdFromBasicId]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Meet Shah
-- Create date: 29 Dec 2016
-- Description:	Get first tagged model from basic id.
-- =============================================
CREATE PROCEDURE [dbo].[GetTaggedModelIdFromBasicId] 
	-- Add the parameters for the stored procedure here
	@BasicId INT = 0 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 ModelId
	FROM Con_EditCms_Cars WITH(NOLOCK)
	WHERE BasicId = @BasicId
END
GO
