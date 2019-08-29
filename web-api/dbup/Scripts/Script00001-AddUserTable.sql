BEGIN TRANSACTION;

CREATE TABLE users (
    userId BIGSERIAL,
    firstName VARCHAR(500) NOT NULL,
    lastName VARCHAR(500) NULL,
    position VARCHAR(500) NULL,
    profilePicture VARCHAR(1024) NULL,
    mobileNumber VARCHAR(20) NULL,
    emailId VARCHAR(250) NULL,
    passwordHash VARCHAR(1024) NOT NULL,
    passwordKey VARCHAR(1024) NOT NULL,
    dateAdded TIMESTAMP NOT NULL DEFAULT (now() AT TIME ZONE 'UTC'),
    dateModified TIMESTAMP NOT NULL DEFAULT (now() AT TIME ZONE 'UTC'),
    CONSTRAINT PK_users_userId PRIMARY KEY (userId),
    CONSTRAINT UK_users_mobileNumber UNIQUE (mobileNumber),
    CONSTRAINT UK_users_emailId UNIQUE (emailId)
);

CREATE TABLE userRoles (
    userRoleId BIGSERIAL NOT NULL,
    userId BIGSERIAL NOT NULL,
    roleName VARCHAR(50) NOT NULL,
    dateAdded TIMESTAMP NOT NULL DEFAULT (now() AT TIME ZONE 'UTC'),
    dateModified TIMESTAMP NOT NULL DEFAULT (now() AT TIME ZONE 'UTC'),
    CONSTRAINT PK_userRoles_userRoleId PRIMARY KEY (userRoleId),
    CONSTRAINT FK_userRoles_userId FOREIGN KEY (userId) REFERENCES users (userId)
);

COMMIT TRANSACTION;