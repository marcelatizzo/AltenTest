USE Hotel;

CREATE TABLE reservation (
    id integer not null auto_increment primary key,
    guest_name varchar(300) not null,
    accomodation_start datetime not null,
    accomodation_end datetime not null
);