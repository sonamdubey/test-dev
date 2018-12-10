IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TempTableSpecialUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TempTableSpecialUsers]
GO

	
--	Author		:	Vivek Singh(16th September 2013)

--	Description :-To get Temp Table of all the Special users and Dealers under the Logged in user(Hierarchy Wise)
--	============================================================

CREATE Procedure [dbo].[TC_TempTableSpecialUsers] 
	@LoggedInUser NUMERIC(20,0)
AS
BEGIN
	DECLARE @NodeCode VARCHAR(50);
	DECLARE @MakeId NUMERIC(18,0);
	DECLARE @Level NUMERIC(5,0);
	DECLARE @MinLevel NUMERIC(18,0);
	DECLARE @MaxLevel NUMERIC(18,0);


	SELECT @NodeCode=NodeCode,@MakeId=T.MakeId
	FROM TC_SpecialUsers T 
	WHERE  T.AliasUserId=@LoggedInUser
	AND T.TC_SpecialUsersId=@LoggedInUser;

	--GET all the users and dealers reporting to the logged in user.
	SELECT TC_SpecialUsersId, TC.UserName, TC.NodeCode, TC.ReportsTo, TC.lvl,'' AS ZoneName,0 AS IsDealer
	INTO #TEMPDEALERS 
	FROM TC_SpecialUsers TC 
	WHERE NodeCode 
	LIKE @NodeCode+'%' AND TC.NodeCode<>@NodeCode AND TC.TC_SpecialUsersId = TC.AliasUserId AND TC.IsActive = 1
	UNION
	SELECT D.ID , D.Organization, TC.NodeCode,d.TC_AMId,lvl+1,TBZ.ZoneName,1 AS IsDealer
	FROM TC_SpecialUsers TC 
	INNER JOIN Dealers AS D ON TC.TC_SpecialUsersId = D.TC_AMId AND D.IsDealerActive=1
	INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId
	WHERE NodeCode LIKE @NodeCode+'%'
	ORDER BY NodeCode;

	SELECT  @MaxLevel=MAX(lvl),@MinLevel=MIN(lvl) FROM #TEMPDEALERS;

	WHILE (@MaxLevel>@MinLevel)
		BEGIN
			UPDATE
			T1
			SET
			 T1.zonename = T2.zonename
			FROM
			#TEMPDEALERS T1
			INNER JOIN
			#TEMPDEALERS T2 ON T2.ReportsTo=T1.TC_SpecialUsersId 
			WHERE
			T1.ZoneName=''
			   
			   SET @MaxLevel=@MaxLevel-1
		END

	SELECT * FROM #TEMPDEALERS ORDER BY  ZoneName ;
	CREATE CLUSTERED INDEX Ind_Temp ON #TEMPDEALERS(TC_SpecialUsersId);
	-- Modified by Vivek on 07-10-2013  remove  lvl,UserName from order by clause
	--,lvl,UserName; -- Modified by Vivek on 07-10-2013  add lvl in order by clause

	DROP TABLE #TEMPDEALERS;
END
