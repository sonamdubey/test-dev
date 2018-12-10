IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Dealer_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Dealer_SP]
GO

	-- Modified By:	TEJASHREE PATIL , Chetan Kane (21st June 2012)
-- Create date: 18 May 2012
-- Description:	Fax and Phone no. varchar size increase to 15, previous it was 10
-- Modified By: Tejashree Patil on 22 Nov 2012 at 4 pm : Changed @Address length from 200 to 500
-- EXEC TC_Dealer_SP @DealerAdminId = 1,@Id = NULL,@ContactTime = 7,@ContactPerson= 'Nilesh',@StateId= 1, @CityId=12, @AreaId=14109,@Email='nilesh.r@gmail.com',@Mobile=9095632992,@Fax=NULL,@Address='Mumbai',@Website='',@Pincode=4000007,@Phone=NULL, 
-- @Status=0,@Passwd=NULL,@FirstName='',@LoginId='',@Organization='Majisons Motors',@Longitude='',@Latitude=''
-- Modified By: Nilesh Utture on 17th June, 2013 Addded two paremeters @MailerName, @MailerEmailId
-- =============================================
CREATE PROCEDURE [dbo].[TC_Dealer_SP]
	-- Add the parameters for the stored procedure here
	@DealerAdminId INT,
	@Id INT=NULL,
	--@DealerId NUMERIC(18,0),
	@ContactTime VARCHAR(20),
	@ContactPerson VARCHAR(50),
	@StateId INT,
	@CityId INT,
	@AreaId INT,
	@Email VARCHAR(100),
	@Mobile VARCHAR(10),
	@Fax VARCHAR(15),
	@Address VARCHAR(500),-- Modified By: Tejashree Patil on 22 Nov 2012 at 4 pm
	@Website VARCHAR(100),
	@Pincode VARCHAR(6),
	@Phone VARCHAR(15),
	@Status INT OUTPUT,
	@Passwd VARCHAR(20),
	@FirstName VARCHAR(50),
	@LoginId VARCHAR(50)='',
	@Organization VARCHAR(50),
	
	-- Modification made by Chetan Kane added 2 new parameters for dealer location
	@Longitude VARCHAR(20),
	@Latitude VARCHAR(20),
	@MailerName VARCHAR(200) = NULL,
	@MailerEmailId VARCHAR(200) = NULL
	
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION AddOutlet
	DECLARE @DealerId BIGINT
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
    -- Insert statements for procedure here
	IF(@Id IS NULL)--IF id parameter is null, we inserting new user to TC_Users table
		BEGIN
			-- checking under partiular dealer same branch name  available or not.if not inserting data
			IF NOT EXISTS(SELECT top 1 Id FROM Dealers WHERE Organization= @Organization)
				BEGIN
					-- Modified By: Nilesh Utture on 17th June, 2013
					INSERT INTO Dealers(LoginId, FirstName, Passwd, Organization, ContactHours, ContactPerson,StateId, CityId, AreaId, EmailId, MobileNo, FaxNo, Address1, WebsiteUrl, PhoneNo,Pincode,IsMultiOutlet,Longitude,Lattitude,MailerName,MailerEmailId)
					VALUES(@LoginId, @FirstName, @Passwd, @Organization, @ContactTime, @ContactPerson, @StateId, @CityId, @AreaId, @Email, @Mobile, @Fax, @Address, @Website,@Phone,@Pincode,0,@Longitude,@Latitude,@MailerName,@MailerEmailId)
					SET @DealerId = SCOPE_IDENTITY()
					--inserting record in TC-DealerAdminMapping table
					INSERT INTO TC_DealerAdminMapping(DealerAdminId,DealerId)
					VALUES(@DealerAdminId,@DealerId)
					
					IF(@DealerId IS NOT NULL)
					BEGIN
						UPDATE Dealers SET IsTCDealer=1 WHERE ID=@DealerId
					END
					set @Status=1
				END
		END
	Else --IF id contaiing data, we updatig branch information
		BEGIN
			-- Modified By: Nilesh Utture on 17th June, 2013
			UPDATE Dealers set Organization=@Organization, ContactHours=@ContactTime, ContactPerson=@ContactPerson,StateId=@StateId, CityId=@CityId, AreaId=@AreaId, EmailId=@Email, MobileNo=@Mobile, FaxNo=@Fax, Address1=@Address, WebsiteUrl=@Website, PhoneNo=@Phone, 
				   Pincode= @Pincode, FirstName=@FirstName,Longitude = @Longitude,Lattitude = @Latitude, MailerName= @MailerName, MailerEmailId = @MailerEmailId WHERE Id = @Id
			set @Status=2
		END
		PRINT @Status
	COMMIT TRANSACTION AddOutlet
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION AddOutlet
		 INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)
         VALUES('TC_Dealer_SP',ERROR_MESSAGE(),GETDATE())
		--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END
