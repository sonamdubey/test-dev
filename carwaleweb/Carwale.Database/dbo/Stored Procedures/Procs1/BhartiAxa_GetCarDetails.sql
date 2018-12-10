IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetCarDetails]
GO

	-- =============================================
-- Author:		<Author,,Ashish Verma>
-- Create date: <Create Date,11 april 2014,>
-- Description:	<Description,For getting car details based on version Id from BhartiAxa_carVersions Table,>
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetCarDetails] 
	-- Add the parameters for the stored procedure here
	
	@CarVersionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--select distinct MANUFACTURE,MODEL,VARIANT from BhartiAxa_CarVersions  where BhartiAxa_CarVersionsId = @CarVersionId
	Select distinct MANUFACTURE,MODEL,VARIANT from BhartiAxa_CarVersions with(nolock) where Reference_No =
    (Select RefrenceId from BhartiAxa_Carwale_MMV with(nolock) where CWVersionId=@CarVersionId)
 --select * from  [CD].[vwMMV] where VersionId = @CarVersionId
END

