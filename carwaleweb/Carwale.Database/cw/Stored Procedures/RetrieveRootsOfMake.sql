IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[RetrieveRootsOfMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[RetrieveRootsOfMake]
GO

	
-- =============================================
-- Author:		<Shikhar>
-- Create date: <27-08-2014>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [cw].[RetrieveRootsOfMake]
	-- Add the parameters for the stored procedure here
	@MakeId numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ROOTID,ROOTNAME FROM CARMODELROOTS WITH (NOLOCK) WHERE MAKEID=@MakeId
END

