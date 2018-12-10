IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarSegments]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarSegments]
GO

	

-- =============================================
-- Author:	Shalini Nair
-- Create date: 28/06/2016
-- Description:	Returns all car segments
-- =============================================
CREATE PROCEDURE [dbo].[GetCarSegments]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Id as Value
		,NAME as Text
	FROM CarSegments WITH (NOLOCK)
	WHERE NAME IS NOT NULL
	ORDER BY NAME
END

