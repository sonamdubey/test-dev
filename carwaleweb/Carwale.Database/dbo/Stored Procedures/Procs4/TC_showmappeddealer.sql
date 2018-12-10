IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_showmappeddealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_showmappeddealer]
GO

	
-- =============================================
-- Author:		<vivek rajak>
-- Create date: <18/05/15>
-- Description:	<to display state,city and mapped dealer on dealerid>
--Modifier: Ajay Singh(2 feb 2016)
-- description: fetched Id of tc_dealeradminmapping table
-- =============================================
CREATE PROCEDURE [dbo].[TC_showmappeddealer] 
	@DealerAdminId varchar(20),
	@DealerId INT = NULL
AS
BEGIN
	
SELECT C.Name AS City,DLR.Organization AS ORG, S.Name AS State ,DLR.Id AS Id,D.Id AS AdminId
	FROM Dealers  AS DLR WITH(NOLOCK)		   
		   INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID = DLR.CityId 
		   INNER JOIN States AS S WITH(NOLOCK) ON S.ID = C.StateId 
		   INNER JOIN TC_DealerAdminMapping AS D WITH(NOLOCK) ON D.DealerId = DLR.ID
	WHERE D.DealerAdminId = @DealerAdminId 
	AND D.DealerId != @DealerId
END



