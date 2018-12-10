IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveServiceCenter]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveServiceCenter]
GO

	CREATE PROCEDURE [dbo].[TC_SaveServiceCenter] @ServiceCenterName VARCHAR(50)
	,@EmailId VARCHAR(60)
	,@Address VARCHAR(250)
	,@AreaId INT = NULL
	,@CityId INT = NULL
	,@ZipCode VARCHAR(10)
	,@PhoneNo VARCHAR(15)
	,@Key VARCHAR(50)
	,@TC_ServiceCenterId INT = 0 OUTPUT
AS
BEGIN
	DECLARE @BranchId INT
		,@UserId INT

	IF (ISNULL(@AreaId, 0) <> 0) -- CHECK FOR IF AreaId EXISTS THEN GET CityId AND StateId
	BEGIN
		SELECT @CityId = C.ID
		FROM Areas AS A WITH (NOLOCK)
		INNER JOIN Cities AS C WITH (NOLOCK) ON C.ID = A.CityId
		WHERE A.ID = @AreaId
	END

	IF (@Key IS NOT NULL)
	BEGIN
		SELECT @BranchId = BranchId
			,@UserId = Id
		FROM TC_Users WITH (NOLOCK)
		WHERE UniqueId = @Key
	END

	IF (
			@TC_ServiceCenterId IS NULL
			OR @TC_ServiceCenterId = 0
			)
	BEGIN
		-- i.e. It is a new Service Center Add Request
		INSERT INTO TC_ServiceCenter (
			ServiceCenterName
			,EmailId
			,Address
			,AreaId
			,CityId
			,ZipCode
			,PhoneNo
			,BranchId
			,CreatedBy
			,CreatedDate
			,LastModifiedDate
			,LastModifiedBy
			,IsActive
			)
		SELECT @ServiceCenterName
			,@EmailId
			,@Address
			,@AreaId
			,@CityId
			,@ZipCode
			,@PhoneNo
			,@BranchId
			,@UserId
			,GETDATE()
			,GETDATE()
			,@UserId
			,1

		SET @TC_ServiceCenterId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE TC_ServiceCenter
		SET ServiceCenterName = @ServiceCenterName
			,EmailId = @EmailId
			,Address = @Address
			,CityId = @CityId
			,AreaId = @AreaId
			,ZipCode = @ZipCode
			,PhoneNo = @PhoneNo
			,BranchId = @BranchId
			,LastModifiedDate = GETDATE()
			,LastModifiedBy = @UserId
		WHERE TC_ServiceCenterId = @TC_ServiceCenterId
	END
END



