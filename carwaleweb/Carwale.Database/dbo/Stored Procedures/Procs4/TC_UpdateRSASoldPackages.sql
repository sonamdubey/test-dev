IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateRSASoldPackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateRSASoldPackages]
GO

	-- ===============================================
-- Author:		Yuga Hatolkar
-- Create date: 10/02/15
-- Description:	Update TC_RSASoldPackages records.
-- ===============================================
CREATE PROCEDURE [dbo].[TC_UpdateRSASoldPackages]

@SoldRSAId INT,
@CustName VARCHAR(50),
@CustMobile VARCHAR(10),
@CustEmail VARCHAR(40),
@CarMakeId INT,
@CarModelId INT,
@CarVersionId INT,
@RegNo VARCHAR(40),
@CustNameNew VARCHAR(50),
@MobileNoNew VARCHAR(10),
@EmailIdNew VARCHAR(40),
@MakeIdNew INT,
@ModelIdNew INT,
@VersionIdNew INT,
@RegNoNew VARCHAR(40),
@MakeYearNew DATETIME,
@MakeYear DATETIME

AS

	BEGIN	

		 UPDATE TC_SoldRSAPackages SET Name = @CustNameNew, MobileNo = @MobileNoNew, Email = @EmailIdNew, 
		 RegistrationNo = @RegNoNew, VersionId = @VersionIdNew, MakeYear = @MakeYearNew WHERE Id = @SoldRSAId

		 INSERT INTO TC_SoldRSAPackagesLog 
		 (SoldRSAId ,Name, MobileNo, EmailId, MakeYear, CarMakeId, CarModelId, CarVersionId,RegistrationNo) 
		 VALUES(@SoldRSAId, @CustName, @CustMobile, @CustEmail, @MakeYear, @CarMakeId, @CarModelId, 
				@CarVersionId, @RegNo)

END
