IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AssignVerifiedDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AssignVerifiedDealers]
GO

	-- =============================================
-- Author		: Vaibhav Kale
-- Create date	: 16-10-2012
-- Description	: Get each Id from the i/p comma separated @Ids paramter
--				: And call the EntryDealers SP by passing all data fetched from table DCRM_VerifiedDealerPool for that Id
-- Modifieer	: Sachin Bharti(22nd March 2013)
-- Purpose		: To insert TC_DealerTypeID of verified dealers in Dealers table
--				  it is passed to ENTRYDEALERS sp as a parameter
-- Modifier		: Sachin Bharti(14th Nov 2013)
-- Purpose		: Pass DCRM_VerifiedDealerPool Id to Entrydealers to update DCRM_VerificationPool with Actual
--				  Dealer_Id when new entry is done in Dealers table for mapping between them
-- Modifier		: Sachin Bharti(27th Jan 2015)
-- Purpose		: Remove hard coded lead soucre value 31 by @LeadSource parameter
-- Modified By : Sunil Yadav On 31 Desc
-- Description : @AppicationId added( to send @AppicationId As a parameter to EntryDealers SP)
-- EXEC DCRM_AssignVerifiedDealers '255',10,50,20,GETDATE(),2
-- Modified BY : Sunil M. Yadav On 20th May 2016, Get dealer details of new added dealer to sync to bikewale database.
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_AssignVerifiedDealers]
	-- Add the parameters for the stored procedure here
	@Ids			VARCHAR(100),
	@SBOExec		INT = NULL,
	@SFieldExec		INT,
	@UpdatedBy		INT,
	@UpdatedOn		DATETIME,
	@ApplicationId INT = 1 -- 1- Default(for carwale) , 2 - Bikewale 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    
    --Declare parameters that are needed to be fetched
    DECLARE
		@ORGANIZATION	VARCHAR(60), 
		@STATEID		NUMERIC, 
		@CITYID			NUMERIC, 
		@FIRSTNAME		VARCHAR(50), 
		@EMAILID		VARCHAR(60),
		@AREAID			NUMERIC = -1, 
		@ADDRESS1		VARCHAR(100),
		@PINCODE		VARCHAR(6), 
		@MOBILENO		VARCHAR(15), 
		@PHONENO		VARCHAR(15), 
		@FAXNO			VARCHAR(15), 
		@CONTACTPERSON	VARCHAR(60), 
		@CONTACTHOURS	VARCHAR(20), 
		@CONTACTEMAIL	VARCHAR(60),
		@CURRENTDATE	DATETIME,
		@EXPIRYDATE		DATETIME,
		@DId			INT,
		@TC_DealerTypeID	INT,
		@LeadSource		SMALLINT,
		@VerifiedDealertblIds VARCHAR(MAX) = @Ids
    
    SET @CURRENTDATE = GETDATE()
    SET @EXPIRYDATE = GETDATE() + 365
    
    --Paramateres to take the individual values from comma separated
	DECLARE @Separator_Position INT, -- This is used to locate each separator character  
			@EachId VARCHAR(100),
			@Separator CHAR(1)=','
	
	SET @Ids = @Ids + @Separator
	
	WHILE CHARINDEX(@Separator, @Ids) <> 0   
	   BEGIN                          
		   --CHARINDEX to check the specific character in the string  
		   SELECT  @Separator_Position = CHARINDEX(@Separator, @Ids) 
		   SELECT  @EachId = LEFT(@Ids, @Separator_position - 1)
	       
		   --Actusal code / functionality starts here
		  -- PRINT @EachId
		   --Fetch the data from DCRM_VerifiedDealerPool for Id obtained from string
		   SELECT 
				@ORGANIZATION = Organization, @FIRSTNAME = Name, @EMAILID = EmailId, @STATEID = StateId, @CITYID = CityId,
				@AREAID = AreaId, @ADDRESS1 = Address1, @PINCODE = Pincode, @MOBILENO = MobileNo, @PHONENO = PhoneNo,
				@FAXNO = FaxNo, @CONTACTPERSON = ContactPerson, @CONTACTHOURS = ContactHours, @CONTACTEMAIL = ContactEmail,
				@LeadSource = LeadSource,@TC_DealerTypeID = TC_DealerTypeId,
				@ApplicationId = ApplicationId
		   FROM DCRM_VerifiedDealerPool(NOLOCK)
		   WHERE ID = @EachId
		   
		   
		   
		   EXEC EntryDealers
				'carwale','carwale',@FIRSTNAME,'',@EMAILID,@ORGANIZATION,@ADDRESS1,NULL,@AREAID,@CITYID,@STATEID,
				@PINCODE,@PHONENO,@FAXNO,@MOBILENO,@CURRENTDATE,@EXPIRYDATE,NULL,@CONTACTPERSON,@CONTACTHOURS,@CONTACTEMAIL,
				1,0,NULL,NULL,0,0,@SBOExec,@SFieldExec,@UpdatedBy,@UpdatedOn,@Did,@LeadSource,@TC_DealerTypeID,
				NULL,NULL,NULL,NULL,NULL,NULL,
				NULL,NULL,NULL,NULL,@LeadSource,NULL,-1,@EachId,@ApplicationId
	       
	       UPDATE DCRM_VerifiedDealerPool SET IsDeleted = 1 , UpdatedBy = @UpdatedBy,UpdatedOn = GETDATE()  WHERE ID = @EachId
	       
		   SELECT  @Ids = STUFF(@Ids, 1, @Separator_Position, '')                                
	   END

	   -- Sunil M. Yadav On 20th May 2016, Get dealer details of new added dealer to sync to bikewale database.
	   -- !!Important -- keep this select statement in last 
	   SELECT D.ID AS DealerId,D.FirstName,D.LastName,D.EmailId,D.Organization,D.Address1,D.Address2,D.AreaId
				,D.CityId,D.StateId,D.Pincode,D.PhoneNo,D.FaxNo,D.MobileNo,D.JoiningDate,D.ExpiryDate,D.WebsiteUrl,D.ContactPerson,D.ContactHours,D.ContactEmail
				,D.TC_DealerTypeId,D.Longitude,D.Lattitude,D.IsDealerActive
	   FROM Dealers D WITH(NOLOCK)
	   JOIN DCRM_VerifiedDealerPool VDP WITH(NOLOCK) ON VDP.Dealer_Id = D.ID 
	   JOIN fnSplitCSV(@VerifiedDealertblIds) AS F ON F.ListMember = VDP.ID
	   WHERE D.ApplicationId = 2 -- only bikewale dealers

END

