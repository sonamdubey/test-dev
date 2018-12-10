IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MicroSite_SelectSpecification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MicroSite_SelectSpecification]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 17-Feb-2011
-- Description:	Fetching data for car features
-- =============================================
CREATE PROCEDURE [dbo].[MicroSite_SelectSpecification]
	-- Add the parameters for the stored procedure here
	@CarVersionId BigInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT NS.*, NF.* FROM NewCarSpecifications NS, NewCarStandardFeatures NF 
	WHERE NF.CarVersionId=NS.CarVersionid AND NS.CarVersionId=@CarVersionId
END
