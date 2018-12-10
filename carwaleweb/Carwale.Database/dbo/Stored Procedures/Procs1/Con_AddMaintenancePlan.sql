IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddMaintenancePlan]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddMaintenancePlan]
GO

	




CREATE  PROCEDURE [dbo].[Con_AddMaintenancePlan]
    @Id				NUMERIC,
	@PlanId			NUMERIC,
    @PartId			NUMERIC,
    @Type			INT,
    @IndDistance	NUMERIC,
    @IndTime		NUMERIC,
    @EntryDate		DATETIME,
    @Status			SMALLINT OUTPUT
 AS
   
BEGIN
     IF @Id = -1
		BEGIN
		     INSERT INTO Con_MaintenancePlan
		             (
		                PlanId, PartId, Type, 
						IndDistance, IndTime, EntryDate
		             )
		      VALUES
		             (
						@PlanId, @PartId, @Type, 
						@IndDistance, @IndTime, @EntryDate
		             )

  				SET @Status = 1
		END
    ELSE
        BEGIN
			UPDATE Con_MaintenancePlan SET 
				PlanId = @PlanId, PartId = @PartId, 
				Type = @Type, IndDistance = @IndDistance, IndTime = @IndTime
			WHERE Id = @Id
            	
			SET @Status = 1
        END
END



