IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SMSDetailsFetch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SMSDetailsFetch]
GO

	-- Created By: Manish Chourasiya on 23-09-2013
-- Description: For sending daily SMS to Special Users regrading their leads for Today and month till today.
-- Modified By : Sachin Bharti(27th Sep 2013)
-- Purpose	:	Added columns for special users Designation and userName
-- Modified by : Manish on 27-09-2013 added separate query for returing the user where alias userid is not match
-- Modified BY : Sachin Bharti on 29th Oct 2013 Make Union All to get data for all the Dealers
--============================================================
CREATE PROCEDURE  [dbo].[TC_SMSDetailsFetch]
AS 
BEGIN
	SELECT S.Mobile Mobile,
	S.TC_SpecialUsersId TC_SpecialUsersId , TD.Designation AS userlevel, --Added by Deepak(27th Sep 2013)
	S.UserName UserName,--Added by Sachin Bharti(27th Sep 2013),
	'0' IsDealer,
	SUM( CASE WHEN TC_TargetTypeId =1 THEN  CurrentDayCount  END ) AS CurrentDayTotalInquiry,
	SUM( CASE WHEN TC_TargetTypeId =1 THEN  CurrentMonthCount END ) AS CurrentMonthTotalInquiry,
	SUM( CASE WHEN TC_TargetTypeId =2 THEN  CurrentDayCount  END) AS CurrentDayTotalTD,
	SUM( CASE WHEN TC_TargetTypeId =2 THEN  CurrentMonthCount END) AS CurrentMonthTotalTD,
	SUM( CASE WHEN TC_TargetTypeId =3 THEN  CurrentDayCount END) AS CurrentDayTotalBookings,
	SUM( CASE WHEN TC_TargetTypeId =3 THEN  CurrentMonthCount END) AS CurrentMonthTotalBookings,
	SUM( CASE WHEN TC_TargetTypeId =4 THEN  CurrentDayCount END) AS CurrentDayTotalRetails,
	SUM( CASE WHEN TC_TargetTypeId =4 THEN  CurrentMonthCount END) AS CurrentMonthTotalRetails,
	SUM( CASE WHEN TC_TargetTypeId =5 THEN  CurrentDayCount END) AS CurrentDayTotalDelivery,
	SUM( CASE WHEN TC_TargetTypeId =5 THEN  CurrentMonthCount END) AS CurrentMonthTotalDelivery
	FROM 
	( SELECT distinct  d.ID,TSU1.TC_SpecialUsersId
		FROM DEALERS as D WITH (NOLOCK)
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = 20 AND  D.IsDealerActive= 1
		INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_AMId=TSU.TC_SpecialUsersId 
		INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON TSU1.NodeCode =SUBSTRING (TSU.NodeCode, 1, LEN(TSU1.NodeCode))
		  WHERE tSU1.IsActive=1
	)  A
	INNER JOIN TC_SpecialUsers AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=A.TC_SpecialUsersId
	INNER JOIN  TC_SMSDetail AS SMS  WITH (NOLOCK) ON SMS.DealerId=A.ID
	INNER JOIN TC_Designation TD WITH (NOLOCK) ON TD.TC_DesignationId = S.Designation
	WHERE S.TC_SpecialUsersId=S.AliasUserId
	AND S.Mobile IS NOT NULL
	AND Mobile <>''
	GROUP BY S.Mobile,S.TC_SpecialUsersId,S.UserName, TD.Designation

	--Added by Sachin Bharti(29th Oct 2013)
	UNION ALL 
	SELECT D.MobileNo Mobile,
	D.Id TC_SpecialUsersId , 'DP' AS userlevel,D.DealerCode UserName,
	'1' IsDealer,
	SUM( CASE WHEN TC_TargetTypeId =1 THEN  CurrentDayCount  END ) AS CurrentDayTotalInquiry,
	SUM( CASE WHEN TC_TargetTypeId =1 THEN  CurrentMonthCount END ) AS CurrentMonthTotalInquiry,
	SUM( CASE WHEN TC_TargetTypeId =2 THEN  CurrentDayCount  END) AS CurrentDayTotalTD,
	SUM( CASE WHEN TC_TargetTypeId =2 THEN  CurrentMonthCount END) AS CurrentMonthTotalTD,
	SUM( CASE WHEN TC_TargetTypeId =3 THEN  CurrentDayCount END) AS CurrentDayTotalBookings,
	SUM( CASE WHEN TC_TargetTypeId =3 THEN  CurrentMonthCount END) AS CurrentMonthTotalBookings,
	SUM( CASE WHEN TC_TargetTypeId =4 THEN  CurrentDayCount END) AS CurrentDayTotalRetails,
	SUM( CASE WHEN TC_TargetTypeId =4 THEN  CurrentMonthCount END) AS CurrentMonthTotalRetails,
	SUM( CASE WHEN TC_TargetTypeId =5 THEN  CurrentDayCount END) AS CurrentDayTotalDelivery,
	SUM( CASE WHEN TC_TargetTypeId =5 THEN  CurrentMonthCount END) AS CurrentMonthTotalDelivery
	from TC_SMSDetail TC(NOLOCK)
	INNER JOIN Dealers D (NOLOCK) ON  D.Id = TC.DealerId
	GROUP BY D.MobileNo,D.ID,D.DealerCode

-----------Manish on 27-09-2013 added separate query for returing the user where alias userid is not match
	SELECT  TC_SpecialUsersId,UserName,Mobile,AliasUserId,'0' IsDealer
	FROM TC_SpecialUsers
	WHERE IsActive=1
	AND  Mobile IS NOT NULL
	AND  TC_SpecialUsersId<>AliasUserId
	AND  Mobile <>''
-------------------------------------------------------------------------------------------

END 
