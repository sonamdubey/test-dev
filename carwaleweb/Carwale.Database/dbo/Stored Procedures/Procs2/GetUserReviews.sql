IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserReviews]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 10/09/14
-- Description: Gets the User reviews 
-- =============================================
CREATE PROCEDURE [dbo].[GetUserReviews] 
	-- Add the parameters for the stored procedure here
@MakeId int = null,
@ModelId int = null,
@Count int = 0

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 -- Insert statements for procedure here

	SELECT Top (@Count) CR.Id, CR.Title, CR.OverallR, CR.Pros, CR.Cons, CR.Comments, CR.EntryDateTime,CMO.MaskingName,
				CMA.Name + ' ' + CMO.Name AS Car, CMA.Name as Make, CMO.Name as Model,
				C.Name AS CustomerName, ISNULL(UP.HandleName,'') AS HandleName,
				isnull(fm.Posts,0) as CommentsCount
				FROM
				CustomerReviews CR with(nolock)
				LEFT JOIN Forum_ArticleAssociation Fso	WITH(NOLOCK) ON CR.ID = Fso.ArticleId 
                LEFT JOIN Forums Fm WITH(NOLOCK) ON Fso.ThreadId = Fm.ID 
				,CarModels CMO with(nolock)
				, CarMakes CMA with(nolock)
				,Customers C with(nolock) LEFT JOIN UserProfile UP with(nolock) ON C.Id = UP.UserId
				WHERE 
				CR.IsActive = 1 AND CR.IsVerified = 1 AND CR.CustomerId <> -1
				AND CR.ModelId = CMO.ID
				AND CR.MakeId = CMA.ID
				AND CR.CustomerId = C.Id
				AND (@MakeId IS NULL OR CR.MakeId = @MakeId)
				AND (@ModelId IS NULL OR CR.ModelId = @ModelId)
				ORDER BY CR.ID DESC
				
				end

