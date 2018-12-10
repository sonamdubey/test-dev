IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_AssignInactiveDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_AssignInactiveDealers]
GO

	-- =============================================
-- Author		: Sachin Bharti
-- Create date	: 14-02-2013
-- Description	: Get each Id from the i/p comma separated @Ids paramter
--				: And call the EntryDealers SP by passing all data fetched from table DCRM_Dealers for that Id
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_AssignInactiveDealers]
	-- Add the parameters for the stored procedure here
	@Ids			VARCHAR(MAX),
	@SBOExec		INT = NULL,
	@SFieldExec		INT,
	@UpdatedBy		INT,
	@UpdatedOn		DATETIME
	
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
		@LeadSource		SMALLINT
    
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
		   PRINT @EachId
		   --Fetch the data from Dealers for Id obtained from string
		   SELECT 
				@ORGANIZATION = Organization, @FIRSTNAME = FirstName, @EMAILID = EmailId, @STATEID = StateId, @CITYID = CityId,
				@AREAID = AreaId, @ADDRESS1 = Address1, @PINCODE = Pincode, @MOBILENO = MobileNo, @PHONENO = PhoneNo,
				@FAXNO = FaxNo, @CONTACTPERSON = ContactPerson, @CONTACTHOURS = ContactHours, @CONTACTEMAIL = ContactEmail,
				@LeadSource = DealerSource
		   FROM Dealers
		   WHERE ID = @EachId
		   
		   EXEC EntryDealers
				'carwale','carwale',@FIRSTNAME,'',@EMAILID,@ORGANIZATION,@ADDRESS1,NULL,@AREAID,@CITYID,@STATEID,
				@PINCODE,@PHONENO,@FAXNO,@MOBILENO,@CURRENTDATE,@EXPIRYDATE,NULL,@CONTACTPERSON,@CONTACTHOURS,@CONTACTEMAIL,
				1,0,NULL,NULL,0,0,@SBOExec,@SFieldExec,@UpdatedBy,@UpdatedOn,@Did,31,
				NULL,NULL,NULL,NULL,NULL,NULL,
				NULL,NULL,NULL,NULL,@LeadSource,NULL,-1
	       
		   SELECT  @Ids = STUFF(@Ids, 1, @Separator_Position, '')                                
	   END
END