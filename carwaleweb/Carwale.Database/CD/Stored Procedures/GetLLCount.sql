IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetLLCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetLLCount]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [CD].[GetLLCount] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ModelId,min(Price) MinPrice,COUNT(ProfileId) Count FROM LiveListings WITH(NOLOCK) GROUP BY ModelId
END
