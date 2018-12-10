IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncDealersWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncDealersWithMysql]
GO

	-- =============================================
-- Author:		<Prasad Gawde>
-- Create date: <Create Date,20/10/2016>
-- Description:	<Description, syncing dealers with mysql>
-- =============================================
CREATE PROCEDURE  [dbo].[SyncDealersWithMysql]
@ID	decimal(18,0),
@EmailId	varchar(250),
@Organization	varchar(100),
@Address1	varchar(500),
@AreaId	decimal(18,0),
@CityId	decimal(18,0),
@StateId	decimal(18,0),
@Pincode	varchar(6),
@PhoneNo	varchar(50),
@FaxNo	varchar(50),
@MobileNo	varchar(50),
@WebsiteUrl	varchar(100),
@ContactPerson	varchar(200),
@Status	tinyint,
@Longitude	float,
@Lattitude	float,
@ApplicationId	tinyint, 
@ShowroomStartTime	varchar(30),
@ShowroomEndTime	varchar(30),
@DealerLastUpdatedBy	int,
@LandlineCode	varchar(4),
@InsertType int = 1,
@LOGINID varchar(30) =null,
@PASSWORD varchar(20) = null,
@FIRSTNAME varchar(50) = null,
@LASTNAME varchar(50) =null,
@ADDRESS2 varchar(100) = null,
@JOININGDATE datetime = null,
@EXPIRYDATE datetime = null,
@CONTACTHOURS varchar(20),
@Source int = null,
@TC_DealerTypeID int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	begin try
		if @InsertType=1 
			INSERT INTO mysql_test...dealers (
					ID,
					FirstName
					,EmailId
					,Organization
					,Address1
					,Address2
					,AreaId
					,CityId
					,StateId
					,Pincode
					,MobileNo
					,FaxNo
					,PhoneNo
					,LandlineCode
					,JoiningDate
					,ExpiryDate
					,WebsiteUrl
					,ContactPerson
					,ContactEmail
					,LogoUrl
					,[ROLE]
					,[STATUS]
					,TC_DealerTypeId
					,Lattitude
					,Longitude
					,ShowroomStartTime
					,ShowroomEndTime
					,DealerCreatedOn
					,DealerCreatedBy
					,LastUpdatedOn
					,DealerLastUpdatedBy
					,ApplicationId --Mihir A Chheda [21-09-2016]
					,IsDealerActive
					)
				VALUES (@Id,
					@ContactPerson
					,@EmailId
					,@Organization
					,@Address1
					,NULL
					,@AreaId
					,@CityId
					,@StateId
					,@Pincode
					,@MobileNo
					,@FaxNo
					,@PhoneNo
					,@LandLineCode
					,GETDATE()
					,NULL
					,@WebsiteUrl
					,@ContactPerson
					,@EmailId
					,NULL
					,'DEALERS'
					,case when @STATUS = 1 then 0 else 1 end
					,2 -- 2 implies NCD
					,@Lattitude
					,@Longitude
					,@ShowroomStartTime
					,@ShowroomEndTime
					,GETDATE()
					,@DealerLastUpdatedBy
					,GETDATE()
					,@DealerLastUpdatedBy
					,@ApplicationId
					,@STATUS
					)
				else if @InsertType = 2
				
						INSERT INTO mysql_test...dealers(ID, LoginId, Passwd, FirstName, LastName, 
							EmailId, Organization, Address1, Address2, AreaId,
							CityId, StateId, Pincode, PhoneNo, FaxNo, 
							MobileNo, JoiningDate, ExpiryDate, WebsiteUrl, 
							ContactPerson, ContactHours, ContactEmail, LogoUrl, ROLE, Status, Preference, DealerSource,TC_DealerTypeID,ApplicationId, IsDealerActive)
						VALUES(@ID,@LOGINID, @PASSWORD, @FIRSTNAME, 
							@LASTNAME, @EMAILID, @ORGANIZATION, 
							@ADDRESS1, @ADDRESS2,@AREAID, @CITYID, 
							@STATEID, @PINCODE, @PHONENO, 
							@FAXNO, @MOBILENO, @JOININGDATE, @EXPIRYDATE, 
							@WEBSITEURL, @CONTACTPERSON, @CONTACTHOURS, 
							@EmailId, '', 'DEALERS', @STATUS, 0, @Source,@TC_DealerTypeID,@ApplicationId, case when @STATUS = 1 then 0 else 1 end)
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncDealersWithMysql',ERROR_MESSAGE(),'dealers',@Id,GETDATE(),@InsertType)
	END CATCH
END

