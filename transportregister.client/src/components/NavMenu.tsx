import { Component, ContextType } from 'react';
import { Collapse, Dropdown, DropdownItem, DropdownMenu, DropdownToggle, NavLink, NavItem, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { Link } from 'react-router-dom';
import AuthContext from '../auth/AuthContext';

interface NavMenuState {
  collapsed: boolean;
  driverDropOpen: boolean;
  vehicleDropOpen: boolean;
  offenceDropOpen: boolean;
  theftDropOpen: boolean;
  userDropOpen: boolean;
}

export class NavMenu extends Component<object, NavMenuState> {
  static displayName = NavMenu.name;
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;

  constructor(props: object) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      driverDropOpen: false,
      vehicleDropOpen: false,
      offenceDropOpen: false,
      theftDropOpen: false,
      userDropOpen: false,
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  toggleDriverDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      driverDropOpen: !prevState.driverDropOpen,
    }));
  };

  toggleVehicleDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      vehicleDropOpen: !prevState.vehicleDropOpen,
    }));
  };

  toggleOffenceDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      offenceDropOpen: !prevState.offenceDropOpen,
    }));
  };

  toggleTheftDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      theftDropOpen: !prevState.theftDropOpen,
    }));
  };

  toggleUserDropdown = () => {
    this.setState((prevState: NavMenuState) => ({
      userDropOpen: !prevState.userDropOpen,
    }));
  };

  render() {
    if (!this.context) {
      return null;
    }

    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand className="me-5" tag={Link} to="/">RVŘP</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          {(this.context.isLoggedIn && this.context.isAdmin) &&
            (
              <Collapse className="d-sm-inline-flex navbarDropdownBar" isOpen={!this.state.collapsed} navbar>
                <ul className="navbar-nav">
                  <NavItem>
                    <NavLink tag={Link} to="/users">Uživatelé</NavLink>
                  </NavItem>
                </ul>

                <ul className="navbar-nav ms-auto">
                  <Dropdown isOpen={this.state.userDropOpen} toggle={this.toggleUserDropdown}>
                    <DropdownToggle nav caret>
                      Přihlášen jako: {this.context.email}
                    </DropdownToggle>
                    <DropdownMenu>
                      <DropdownItem tag={Link} onClick={this.context.logout}>Odhlásit se</DropdownItem>
                    </DropdownMenu>
                  </Dropdown>
                </ul>
              </Collapse>
            )}
          {(this.context.isLoggedIn && !this.context.isAdmin) &&
            (
              <Collapse className="d-sm-inline-flex navbarDropdownBar" isOpen={!this.state.collapsed} navbar>
                <ul className="navbar-nav">
                  <Dropdown isOpen={this.state.driverDropOpen} toggle={this.toggleDriverDropdown} >
                    <DropdownToggle nav caret>
                      Řidiči
                    </DropdownToggle>
                    <DropdownMenu>
                      <DropdownItem header>Řidiči</DropdownItem>
                      <DropdownItem tag={Link} to="/driverSearch">Vyhledat</DropdownItem>
                      <DropdownItem tag={Link} to="/driverAll">Zobrazit všechny</DropdownItem>
                    </DropdownMenu>
                  </Dropdown>

                  <Dropdown isOpen={this.state.vehicleDropOpen} toggle={this.toggleVehicleDropdown}>
                    <DropdownToggle nav caret>
                      Vozidla
                    </DropdownToggle>
                    <DropdownMenu>
                      <DropdownItem header>Vozidla</DropdownItem>
                      <DropdownItem tag={Link} to="/vehicle/search">Vyhledat</DropdownItem>
                      <DropdownItem tag={Link} to="/">Zobrazit všechny</DropdownItem>
                      {this.context.isOfficial && (
                        <DropdownItem tag={Link} to="/vehicle/create">Registrovat vozidlo</DropdownItem>
                      )}
                    </DropdownMenu>
                  </Dropdown>

                  <Dropdown isOpen={this.state.offenceDropOpen} toggle={this.toggleOffenceDropdown}>
                    <DropdownToggle nav caret>
                      Přestupky
                    </DropdownToggle>
                    <DropdownMenu>
                      <DropdownItem header>Přestupky</DropdownItem>
                      <DropdownItem tag={Link} to="/offencePending">Zobrazit nevyřešené</DropdownItem>
                      <DropdownItem tag={Link} to="/offenceAll">Zobrazit všechny</DropdownItem>
                    </DropdownMenu>
                  </Dropdown>

                  <Dropdown isOpen={this.state.theftDropOpen} toggle={this.toggleTheftDropdown}>
                    <DropdownToggle nav caret>
                      Kradená vozidla
                    </DropdownToggle>
                    <DropdownMenu>
                      <DropdownItem header>Kradená vozidla</DropdownItem>
                      <DropdownItem tag={Link} to="/theftsActive">Zobrazit aktuální</DropdownItem>
                      <DropdownItem tag={Link} to="/thefts">Zobrazit všechny</DropdownItem>
                    </DropdownMenu>
                  </Dropdown>
                </ul>

                <ul className="navbar-nav ms-auto">
                  <Dropdown isOpen={this.state.userDropOpen} toggle={this.toggleUserDropdown}>
                    <DropdownToggle nav caret>
                      Přihlášen jako: {this.context.email}
                    </DropdownToggle>
                    <DropdownMenu>
                      <DropdownItem tag={Link} onClick={this.context.logout}>Odhlásit se</DropdownItem>
                    </DropdownMenu>
                  </Dropdown>
                </ul>
              </Collapse>
            )}
        </Navbar>
      </header>
    );
  }
}
