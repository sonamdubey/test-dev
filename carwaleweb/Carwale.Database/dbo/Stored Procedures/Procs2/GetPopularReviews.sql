IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPopularReviews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPopularReviews]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 30.09.2013
-- Description:	Get Popular User reviews for top selling cars
-- Modified By : Akansha on 06.03.2014 Added masking name column
-- =============================================
CREATE PROCEDURE [dbo].[GetPopularReviews] 
	@Top int = 3
AS
BEGIN
With CTE AS(
	SELECT top 3 
	CMD.id, 
	cmk.name + ' ' + cmd.name as CarName, 
	CMD.HostURL, 
	CMD.SmallPic, 
	CMK.Name as MakeName,
	CMD.MaskingName,
	TSC.EntryDate,SortOrder
	FROM con_topsellingcars TSC WITH(NOLOCK)
	JOIN carmodels CMD WITH(NOLOCK)  ON TSC.modelid = CMD.id
	Join CarMakes CMK  WITH(NOLOCK)on CMD.CarMakeId=CMK.ID
	WHERE TSC.Status = 1 AND 
	ISNULL(CMD.ReviewCount, 0) > 0 
	Order By  SortOrder)

SELECT * FROM(
	SELECT *, 
	ROW_NUMBER() over(partition by Id order by NewID())  as rowno 
	FROM (
	SELECT TOP 10 
	RV.Title,
	RV.Liked, 
	CT.id, 
	CarName , 
	MaskingName,
	MakeName,
	CT.HostURL, 
	CT.SmallPic, 
	RV.EntryDateTime,
	RV.OverallR,
	Rv.CustomerId,
	SortOrder,
	Rv.ID as ReviewID,
	C.Name as CustName
	FROM CTE CT WITH(NOLOCK) INNER JOIN CustomerReviews Rv  WITH(NOLOCK) ON Rv.ModelId = CT.ID
	inner join Customers C on CustomerId=c.Id
	WHERE RV.IsActive = 1 
	ORDER BY Liked DESC) t ) b
	WHERE rowno = 1
	ORDER BY SortOrder
END
