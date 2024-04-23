import { Component, ContextType } from 'react';
import { Collapse, Dropdown, DropdownItem, DropdownMenu, DropdownToggle, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { Link } from 'react-router-dom';
import AuthContext from '../auth/AuthContext';


// TODO navbar when NavbarToggle - page is small (in hamburger menu)
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
    console.log(this.context.isAdmin);
    console.log(this.context.isOfficial);
    console.log(this.context.isOfficer);

    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand className="me-5" tag={Link} to="/">RVŘP</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          {this.context.isLoggedIn && (
            <Collapse className="d-sm-inline-flex navbarDropdownBar" isOpen={!this.state.collapsed} navbar>
              {/*<div className="navbarDropdownBar">*/}
              <ul className="navbar-nav">
                <Dropdown isOpen={this.state.driverDropOpen} toggle={this.toggleDriverDropdown} >
                  <DropdownToggle nav caret>
                    Řidiči
                  </DropdownToggle>
                  <DropdownMenu>
                    <DropdownItem header>Řidiči</DropdownItem>
                    <DropdownItem tag={Link} to="/driverSearch">Vyhledat</DropdownItem>
                    <DropdownItem tag={Link} to="/driverCreate">Zaregistrovat</DropdownItem>
                    <DropdownItem>?Zobrazit včechny</DropdownItem>
                  </DropdownMenu>
                </Dropdown>

                <Dropdown isOpen={this.state.vehicleDropOpen} toggle={this.toggleVehicleDropdown}>
                  <DropdownToggle nav caret>
                    Vozidla
                  </DropdownToggle>
                  <DropdownMenu>
                    <DropdownItem header>Vozidla</DropdownItem>
                    <DropdownItem tag={Link} to="/vehicleSearch">Vyhledat</DropdownItem>
                    <DropdownItem tag={Link} to="/vehicleCreate">-Úředník - Registrovat</DropdownItem>
                    <DropdownItem>?Zobrazit včechny</DropdownItem>
                  </DropdownMenu>
                </Dropdown>

                <Dropdown isOpen={this.state.offenceDropOpen} toggle={this.toggleOffenceDropdown}>
                  <DropdownToggle nav caret>
                    Přestupky
                  </DropdownToggle>
                  <DropdownMenu>
                    <DropdownItem header>Přestupky</DropdownItem>
                    <DropdownItem tag={Link} to="/offenceCreate">-Policista - registrovat</DropdownItem>
                    <DropdownItem tag={Link} to="/offencePending">-Úředník - Zobrazit nevyřešené (své)</DropdownItem>
                    <DropdownItem>?Vyhledat (asi ne, spíš sobrazit u řidiče TODO)</DropdownItem>
                    <DropdownItem>?Zobrazit včechny</DropdownItem>
                  </DropdownMenu>
                </Dropdown>

                <Dropdown isOpen={this.state.theftDropOpen} toggle={this.toggleTheftDropdown}>
                  <DropdownToggle nav caret>
                    Kradená vozidla
                  </DropdownToggle>
                  <DropdownMenu>
                    <DropdownItem header>Kradená vozidla</DropdownItem>
                    <DropdownItem tag={Link} to="/theftCreate">Evidovat krádež</DropdownItem>
                    <DropdownItem tag={Link} to="/theftFound">-Policista - nahlásit nález</DropdownItem>
                    <DropdownItem>?Vyhledat</DropdownItem>
                    <DropdownItem>?Listina/zobrazit vše (v zadání ano, ale má smysl?)</DropdownItem>
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
