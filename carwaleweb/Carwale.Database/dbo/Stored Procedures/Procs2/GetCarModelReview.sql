IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarModelReview]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarModelReview]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 14/08/14
-- Description:	Gets the CarModel Review based on the modelId passed 
-- =============================================
CREATE PROCEDURE [dbo].[GetCarModelReview]
	-- Add the parameters for the stored procedure here
	@ModelId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CS.SmallDescription,CS.FullDescription
                 FROM CarSynopsis AS CS  WITH (NOLOCK)
                WHERE CS.ModelId = @ModelId AND CS.IsActive = 1
END


