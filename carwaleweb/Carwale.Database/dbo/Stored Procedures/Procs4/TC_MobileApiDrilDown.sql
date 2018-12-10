IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MobileApiDrilDown]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MobileApiDrilDown]
GO

	-- =============================================
-- Author:		Vishal Srivastava
-- Create date: 13/02/2014 1628 hrs ist
-- Description:	To get the list of users under a users
-- =============================================
CREATE PROCEDURE [dbo].[TC_MobileApiDrilDown] 
	-- Add the parameters for the stored procedure here
	@userId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Designation int = null
    -- Insert statements for procedure here

	SELECT @Designation=U.Designation, @UserId=TC_SpecialUsersId FROM TC_SpecialUsers AS U WHERE U.TC_SpecialUsersId=@userId

	IF(@Designation=4)
		BEGIN
			SELECT DISTINCT U.Id AS UserId,
			U.UserName , 'False' AS SpecialUser
			FROM DEALERS as D WITH (NOLOCK)
			JOIN TC_Users AS U WITH(NOLOCK) ON D.ID=U.BranchId
			INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = 20 
														AND  D.IsDealerActive= 1
			INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_AMId=TSU.TC_SpecialUsersId 
			INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON TSU1.NodeCode =SUBSTRING (TSU.NodeCode, 1, LEN(TSU1.NodeCode))
			WHERE tSU1.IsActive=1 AND U.lvl=1
			AND tsu1.TC_SpecialUsersId= @UserId
		END
	ELSE
		BEGIN
			select DISTINCT SU1.TC_SpecialUsersId as UserId, SU1.UserName, 'True' AS SpecialUser 
			from TC_SpecialUsers as SU1 WITH(NOLOCK)
			JOIN TC_SpecialUsers AS SU2 WITH(NOLOCK) ON SU1.ReportsTo=SU2.AliasUserId
			WHERE SU2.TC_SpecialUsersId=@UserId AND SU1.IsActive=1 and SU2.IsActive=1
		END
END
