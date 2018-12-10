IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetYear]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetYear]
GO

	-- =============================================
-- Author:		Vishal Srivastava
-- Create date: December 5 2013 1441 HRS IST
-- Description:	Created to get the year from TC_TMIntermediateLegacyDetail 
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMGetYear] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 [YEAR]
		FROM TC_TMIntermediateLegacyDetail WITH(NOLOCK)
END
