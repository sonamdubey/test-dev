IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateFinanceInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateFinanceInquiry]
GO

	
--THIS PROCEDURE IS FOR UPDATING RECORDS FOR finance inquiries

CREATE PROCEDURE [dbo].[UpdateFinanceInquiry]
	@Id			NUMERIC,	-- 
	@DOB			DATETIME,
	@ResidenceDuration	INT,
	@Employer		VARCHAR(50),	
	@JobTitle		VARCHAR(50),
	@Income		VARCHAR(50),
	@OfficePhone		VARCHAR(20),
	@OfficeExt		VARCHAR(5),
	@ServiceDuration	INT,
	@PanCardNo		VARCHAR(50),
	@Comments		VARCHAR(2000)

 AS
	BEGIN
		UPDATE FinanceInquiries SET
			DOB			= @DOB,
			LengthResidence	= @ResidenceDuration, 
			Employer		= @Employer,
			JobTitle			= @JobTitle, 
			MonthlyIncome		= @Income, 
			OfficePhone		= @OfficePhone, 
			PhoneExtension		= @OfficeExt, 
			LengthService		= @ServiceDuration,
			PanCardNo		= @PanCardNo, 
			Comments 		= @Comments
		WHERE
			ID = @ID

	END
