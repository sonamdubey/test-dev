IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetExpertReviewsDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetExpertReviewsDetails]
GO

	-- =============================================
-- Author:		Supriya Khartode
-- Create date: 21/7/2014
-- Description:	To get the details of expert-reviews for particular article
-- =============================================
CREATE PROCEDURE [cw].[GetExpertReviewsDetails]
	-- Add the parameters for the stored procedure here
	@BasicId Numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT B.Title, B.AuthorName, B.Url, B.DisplayDate, CMA.Name As Make, CMO.Name As Model,CMO.MaskingName, CV.Name As Version, C.ModelId, C.VersionId,
    CASE WHEN C.VersionId = -1 THEN CMA.Name + ' ' + CMO.Name  ELSE CMA.Name + ' ' + CMO.Name + ' ' + CV.Name END As Car, CA.Name As Category, CF.FieldName, CF.ValueType,
    CASE WHEN CF.ValueType = 1 THEN Cast(OI.BooleanValue As VarChar(1)) WHEN CF.ValueType = 2 THEN Cast(OI.NumericValue As VarChar(15)) WHEN CF.ValueType = 3 THEN Cast(OI.DecimalValue As VarChar)
    WHEN CF.ValueType = 4 THEN OI.TextValue  WHEN CF.ValueType = 5 THEN Convert(VarChar, OI.DateTimeValue, 103) ELSE '' END As OtherInfoValue, ( Cast( P.Priority As VarChar(10) ) + '. ' + P.PageName ) As PageNameForDDL, P.PageName, P.Priority, PC.Data, SC.Name As SubCategory,TG.Tag
    FROM Con_EditCms_Basic B WITH (NOLOCK)
    Left Join Con_EditCms_BasicSubCategories BSC WITH (NOLOCK) On BSC.BasicId = B.Id
    Left Join Con_EditCms_SubCategories SC WITH (NOLOCK) On SC.Id = BSC.SubCategoryId And SC.IsActive = 1
    Left Join Con_EditCms_Cars C WITH (NOLOCK) On C.BasicId = B.Id And C.IsActive = 1
    Left Join Con_EditCms_OtherInfo OI WITH (NOLOCK) On OI.BasicId = B.Id
    Left Join Con_EditCms_CategoryFields CF WITH (NOLOCK) On CF.Id = OI.CategoryFieldId And B.CategoryId = CF.CategoryId
    Left Join Con_EditCms_Category Ca WITH (NOLOCK) On Ca.Id = B.CategoryId
    Left Join Con_EditCms_Pages P WITH (NOLOCK) On P.BasicId = B.Id
    Left Join Con_EditCms_PageContent PC WITH (NOLOCK) On PC.PageId = P.Id
    Left JOIN CarMakes CMA WITH (NOLOCK) On C.MakeId = CMA.ID
    Left JOIN CarModels CMO WITH (NOLOCK) On C.ModelId = CMO.ID
    Left JOIN CarVersions CV WITH (NOLOCK) On C.VersionId = CV.ID
	LEFT JOIN Con_EditCms_BasicTags BT WITH (NOLOCK) ON BT.BasicId = B.Id
	LEFT JOIN Con_EditCms_Tags TG WITH (NOLOCK) ON TG.Id = BT.TagId
    WHERE
    B.ID = @BasicId AND B.ApplicationID =1
    ORDER BY P.Priority 
END

