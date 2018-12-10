IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarTestDriveData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarTestDriveData]
GO

	
CREATE PROCEDURE [dbo].[CRM_SaveCarTestDriveData]

	@CarTestDriveId			Numeric,
	@CarBasicDataId			Numeric,
	@TDLocationId			SmallInt,
	@TDStatusId				SmallInt,
	@DealerId				Numeric,
	@Comments				VarChar(1000),
	@UpdatedById			Numeric,
	
	@TDRequestDate			DateTime,
	@TDCompletedDate		DateTime,
	@CreatedOn				DateTime,
	@UpdatedOn				DateTime,
	@ContactPerson			VarChar(50),
	@Contact				VarChar(50),
	@currentId				Numeric OutPut
				
 AS
BEGIN
	SET @currentId = -1
	UPDATE CRM_CarTestDriveData
	SET TDLocationType = @TDLocationId, TDStatusId = @TDStatusId, DealerId = @DealerId, 
		Comments = @Comments, UpdatedBy = @UpdatedById,TDRequestDate = @TDRequestDate,
		TDCompletedDate = @TDCompletedDate,
		UpdatedOn = @UpdatedOn, ContactPerson = @ContactPerson, Contact = @Contact
	WHERE CarBasicDataId = @CarBasicDataId
			
	IF @@ROWCOUNT = 0
		BEGIN

			INSERT INTO CRM_CarTestDriveData
			(
				CarBasicDataId, TDLocationType, TDStatusId, DealerId, Comments, UpdatedBy,
				TDRequestDate, TDCompletedDate, CreatedOn, UpdatedOn,
				ContactPerson, Contact
			)
			VALUES
			(
				@CarBasicDataId, @TDLocationId, @TDStatusId, @DealerId, @Comments, @UpdatedById,
				@TDRequestDate, @TDCompletedDate, @CreatedOn, @UpdatedOn,
				@ContactPerson, @Contact
			)
			
			SET	@currentId = Scope_Identity()
	
		END 

	ELSE
		
		BEGIN		
			SET	@currentId = @CarTestDriveId
		END
END


