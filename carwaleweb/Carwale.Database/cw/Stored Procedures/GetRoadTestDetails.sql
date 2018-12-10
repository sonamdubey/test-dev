IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetRoadTestDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetRoadTestDetails]
GO

	
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <1/08/2012>
-- Description:	<Returns the latest road test details along with the car name,make and model>
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- =============================================
CREATE PROCEDURE [cw].[GetRoadTestDetails] 
@ApplicationId int
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Top 5 CEB.*, CMA.Name + ' ' + CMO.Name + ' ' + ISNULL(CV.Name, '') As Car,
 CMA.Name As MakeName,          CMO.Name As ModelName
From Con_EditCms_Basic CEB
        Left Join Con_EditCms_Cars CEC WITH (NOLOCK) On CEC.BasicId =                CEB.Id And CEC.IsActive = 1
        Left Join CarVersions CV WITH (NOLOCK) On CV.Id = CEC.VersionId
        Left Join CarModels CMO WITH (NOLOCK) On CMO.Id = CEC.ModelId
        Left Join CarMakes CMA WITH (NOLOCK) On CMA.Id = CMO.CarMakeId
Where CEB.CategoryId = 8 And 
CEB.IsActive = 1 And                    CEB.IsPublished = 1 AND CEB.ApplicationID = @ApplicationId
Order By CEB.DisplayDate Desc
END


