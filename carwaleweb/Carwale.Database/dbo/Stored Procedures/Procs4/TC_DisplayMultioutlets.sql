IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DisplayMultioutlets]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DisplayMultioutlets]
GO

	
-- =============================================
-- Author:		Vivek Rajak
-- Create date: 29/04/2015
-- Description:	To Display Records from TC_DealerAdmin table.
-- Modifier   : Ajay Singh(2 feb 2016)
-- Description : Added the Concept of Group
-- Modifier : Amit Yadav (12th Feb 2016)
-- Purpose : To get the dealer type and can delete for a group/multioutlets
-- EXEC TC_DisplayMultioutlets 1
-- =============================================
CREATE PROCEDURE [dbo].[TC_DisplayMultioutlets]
@IsGroup BIT=0
AS
BEGIN	
	SET NOCOUNT ON;
	--DECLARE @CanDel INT
	IF @IsGroup=1
	BEGIN
	SELECT DISTINCT TD.Id,TD.Organization,TD.IsActive,TD.EntryDate,TD.EmailId,TD.IsEnquiryMail,TD.DealerId,C.Name AS City,S.Name AS State,
	CASE WHEN TD.IsGroup = 1 THEN 'Group' ELSE 'MultiOutlet' END AS IsType,DT.DealerType
	,CASE ISNULL((SELECT TOP 1 DAM.Id FROM TC_DealerAdminMapping DAM WITH(NOLOCK) WHERE DAM.DealerAdminId = TD.Id AND DAM.DealerId<>TD.DealerId AND DAM.IsGroup IS NULL),0) WHEN 0 THEN 1 ELSE 0 END AS CanDelete
	FROM  DEALERS AS D WITH(NOLOCK) 
	INNER JOIN TC_DealerAdmin AS TD WITH(NOLOCK) ON D.ID=TD.DealerId
	INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID = D.CityId
	INNER JOIN States AS S WITH(NOLOCK) ON S.ID = D.StateId
	INNER JOIN TC_DealerType DT WITH(NOLOCK) ON DT.TC_DealerTypeId=D.TC_DealerTypeId --FOR DEALERTYPE
	WHERE D.IsGroup = 1
	AND D.IsMultiOutlet <> 1
	AND D.IsDealerActive = 1
	AND TD.IsActive = 1
	END
	ELSE
	BEGIN
	SELECT DISTINCT TD.Id,TD.Organization,TD.IsActive,TD.EntryDate,TD.EmailId,TD.IsEnquiryMail,TD.DealerId,C.Name AS City,S.Name AS State,
	CASE WHEN TD.IsGroup = 1 THEN 'Group' ELSE 'MultiOutlet' END AS IsType,DT.DealerType
	,CASE ISNULL((SELECT TOP 1 DAM.Id FROM TC_DealerAdminMapping DAM WITH(NOLOCK) WHERE DAM.DealerAdminId = TD.Id AND DAM.DealerId<>TD.DealerId AND DAM.IsGroup IS NULL),0) WHEN 0 THEN 1 ELSE 0 END AS CanDelete
	FROM  Dealers AS D WITH(NOLOCK) 
	INNER JOIN TC_DealerAdmin AS TD WITH(NOLOCK) ON D.ID=TD.DealerId
	INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID = D.CityId
	INNER JOIN States AS S WITH(NOLOCK) ON S.ID = D.StateId
	INNER JOIN TC_DealerType DT WITH(NOLOCK) ON DT.TC_DealerTypeId=D.TC_DealerTypeId --FOR DEALERTYPE
	WHERE D.IsMultiOutlet = 1
	AND D.IsGroup = 0
	AND D.IsDealerActive = 1
	AND TD.IsActive = 1
	END
END

--------------------------------------------------------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------------------------------------
