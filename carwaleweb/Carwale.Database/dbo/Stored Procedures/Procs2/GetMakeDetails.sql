IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMakeDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMakeDetails]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 25/11/14
-- Description:	Fetches the Make details 
-- =============================================
CREATE PROCEDURE [dbo].[GetMakeDetails]
	-- Add the parameters for the stored procedure here
@MakeId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 SELECT Name AS MakeName, ID AS MakeId FROM CarMakes  WITH(NOLOCK)
				WHERE ID = @MakeId 
END

