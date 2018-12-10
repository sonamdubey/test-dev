IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerMake]
GO

	-- Created By:	Umesh Ojha	
-- Create date: 25 feb 2013
-- Description:	Fetching Car makes according to dealerid
-- Modified by: Rakesh Yadav added order by clause on 8 May 2015
CREATE PROCEDURE   [dbo].[Microsite_GetDealerMake]  @DealerId  AS INT
AS 
BEGIN
	SELECT  M.NAME,M.Id
	 FROM CarMakes        AS M  WITH (NOLOCK)
	 JOIN TC_DealerMakes  AS TCD WITH (NOLOCK) ON TCD.MakeId=M.ID
	 WHERE TCD.DealerId=@DealerId
	 ORDER BY M.Name
END 
