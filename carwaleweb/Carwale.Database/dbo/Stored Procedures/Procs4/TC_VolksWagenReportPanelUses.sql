IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_VolksWagenReportPanelUses]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_VolksWagenReportPanelUses]
GO

	

-- =============================================
-- Author:		Deepak
-- Create date: 16-07-2013
-- Description: Report for VW AutoBiz panel Usage 
-- Modified by Manish on 28-06-2013 for adding Test Drive columns
-- Modified By	:	Sachin Bharti(3rd July 2013)
-- =============================================
CREATE PROCEDURE [dbo].[TC_VolksWagenReportPanelUses]-- 3

AS
BEGIN
--Subject :VW AutoBiz panel Usage 19-06-2013 3 PM

SELECT TD.TC_DesignationId AS UId, s.UserName,s.Email,TD.Designation,CONVERT(date,LoggedTime) as [Date],COUNT(distinct id) [logins]
FROM  TC_SpecialUsers AS s JOIN TC_UsersLog AS u ON u.UserId=s.TC_SpecialUsersId
JOIN TC_Designation TD ON S.Designation = TD.TC_DesignationId AND TD.MakeId = 20
where u.BranchId is null
and LoggedTime BETWEEN '2013/07/03' AND  GETDATE() AND s.TC_SpecialUsersId NOT IN(39,40,1,2,42)
group by s.TC_SpecialUsersId,s.UserName,s.Email,TD.Designation,CONVERT(date,LoggedTime), TD.TC_DesignationId
ORDER BY TD.TC_DesignationId, UserName, Date DESC



END
